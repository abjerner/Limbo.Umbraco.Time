// [CHANGE: upgrade to Umbraco 17] Related: index.ts, PropertyEditors/UnixTime/UnixTimestampPropertyEditor.cs
// Replaces the AngularJS "UnixTimestamp.html" view + "Limbo.Umbraco.UnixTimestamp.Controller". The
// picked date is converted to UNIX seconds and stored as a string (the C# editor deliberately uses
// ValueTypes.String so that zero is a storable value), exactly as in v13.
import { css, customElement, html, nothing, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
  UmbPropertyEditorConfigCollection,
  UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { parseBoolean } from './parse-boolean.js';

/** Formats a Date as the "YYYY-MM-DDTHH:mm:ss" string expected by a datetime-local input. */
function toInputValue(date: Date): string {
  const pad = (value: number) => value.toString().padStart(2, '0');
  return (
    `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}` +
    `T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`
  );
}

@customElement('limbo-unix-timestamp-property-editor-ui')
export class LimboUnixTimestampPropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public set value(value: string | undefined) {
    this.#value = value;
    const seconds = Number(value);
    this._inputValue = value && !isNaN(seconds) && seconds !== 0 ? toInputValue(new Date(seconds * 1000)) : '';
  }
  public get value(): string | undefined {
    return this.#value;
  }
  #value?: string;

  @property({ type: Boolean, reflect: true })
  public readonly = false;

  @property({ type: Boolean })
  public mandatory = false;

  @property({ attribute: false })
  public set config(config: UmbPropertyEditorConfigCollection | undefined) {
    // "parseBoolean" - not truthiness/"Boolean" - because v13 data types stored these as "1"/"0"
    // strings. "_configReadonly" is kept separate from "readonly" so that assigning the config never
    // clears the readonly state Umbraco itself sets on the element.
    this._configReadonly = parseBoolean(config?.getValueByAlias('readonly'));
    this._showUnixTimestamp = parseBoolean(config?.getValueByAlias('showUnixTimestamp'));
  }

  @state()
  private _configReadonly = false;

  @state()
  private _inputValue = '';

  @state()
  private _showUnixTimestamp = false;

  #onChange(event: Event & { target: HTMLInputElement }) {
    const value = event.target.value?.toString();
    const date = value ? new Date(value) : undefined;
    this.value = date && !isNaN(date.getTime()) ? Math.floor(date.getTime() / 1000).toString() : undefined;
    this.dispatchEvent(new UmbChangeEvent());
  }

  override render() {
    return html`
      <umb-input-date
        type="datetime-local"
        step="1"
        label=${this.localize.term('placeholders_enterdate')}
        .value=${this._inputValue}
        ?required=${this.mandatory}
        ?readonly=${this.readonly || this._configReadonly}
        @change=${this.#onChange}>
      </umb-input-date>
      ${this._showUnixTimestamp && this.value
        ? html`<div class="timestamp"><strong>Unix timestamp:</strong> ${this.value}</div>`
        : nothing}
    `;
  }

  static override styles = [
    css`
      :host {
        display: block;
      }
      .timestamp {
        margin-top: var(--uui-size-space-2, 6px);
        font-size: 0.85em;
      }
    `,
  ];
}

export default LimboUnixTimestampPropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-unix-timestamp-property-editor-ui': LimboUnixTimestampPropertyEditorUiElement;
  }
}
