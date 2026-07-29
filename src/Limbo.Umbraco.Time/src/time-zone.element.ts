// [CHANGE: upgrade to Umbraco 17] Related: index.ts, Controllers/Api/Management/TimeZoneController.cs
// Replaces the AngularJS "TimeZone.html" config view and its "TimeZoneOverlay.html" picker. Instead
// of the old "/umbraco/backoffice/Limbo/Time/GetTimeZones" endpoint, the time zones are read from
// the Management API (/umbraco/management/api/v1/time/time-zones) through "umbHttpClient", which is
// the backoffice's pre-configured client: it resolves the request against the configured server URL
// and attaches the backoffice credentials. A bare "fetch" of an absolute path would break whenever
// the backoffice is served from a different host than the API ("Umbraco:CMS:Global:BackOfficeHost").
import { customElement, html, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { umbHttpClient } from '@umbraco-cms/backoffice/http-client';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';

interface LimboTimeZone {
  id: string;
  name: string;
}

const DEFAULT_TIME_ZONE = 'local';

@customElement('limbo-time-zone-property-editor-ui')
export class LimboTimeZonePropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public value?: string;

  @property({ type: Boolean, reflect: true })
  public readonly = false;

  @state()
  private _timeZones: Array<LimboTimeZone> = [];

  @state()
  private _error?: string;

  @state()
  private _loading = true;

  constructor() {
    super();
    void this.#loadTimeZones();
  }

  async #loadTimeZones() {
    try {
      const { data, error } = await umbHttpClient.get({
        url: '/umbraco/management/api/v1/time/time-zones',
        security: [{ type: 'http', scheme: 'bearer' }],
      });
      if (error) throw error;
      this._timeZones = (data ?? []) as Array<LimboTimeZone>;
      // NOTE: no default is written back here. Doing so used to dispatch an "UmbChangeEvent" as soon
      // as the data type workspace opened, marking it dirty before the user had touched anything (and
      // racing with Umbraco's own assignment of "value"). An unset time zone already means "server
      // local" server side - see "GetTimeZoneInfo" in the value converters - so the select simply
      // falls back to DEFAULT_TIME_ZONE when rendering.
    } catch (error) {
      this._error = error instanceof Error ? error.message : 'Failed to load time zones';
    } finally {
      // Always clear the loading flag - an empty response must not leave a spinner up forever.
      this._loading = false;
    }
  }

  #onChange(event: Event) {
    this.value = (event.target as HTMLSelectElement).value;
    this.dispatchEvent(new UmbChangeEvent());
  }

  override render() {
    if (this._loading) return html`<uui-loader></uui-loader>`;
    if (this._error) return html`<uui-icon name="icon-alert"></uui-icon> ${this._error}`;

    const selected = this.value ?? DEFAULT_TIME_ZONE;

    return html`
      <uui-select
        .value=${selected}
        ?disabled=${this.readonly}
        .options=${this._timeZones.map((timeZone) => ({
          name: timeZone.name,
          value: timeZone.id,
          selected: timeZone.id === selected,
        }))}
        @change=${this.#onChange}>
      </uui-select>
    `;
  }
}

export default LimboTimeZonePropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-time-zone-property-editor-ui': LimboTimeZonePropertyEditorUiElement;
  }
}
