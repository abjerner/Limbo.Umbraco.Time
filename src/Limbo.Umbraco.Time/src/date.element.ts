// [CHANGE: upgrade to Umbraco 17] Related: datetime.element.ts, index.ts, PropertyEditors/Date/DatePropertyEditor.cs
// Replaces the AngularJS "datepicker" view used by the v13 Limbo Date editor. The stored value
// format is unchanged: "YYYY-MM-DD 00:00:00", which is what Umbraco's own DateTime data editor
// persists, and what DateValueConverter reads back.
import { customElement, html, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';

@customElement('limbo-date-property-editor-ui')
export class LimboDatePropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public set value(value: string | undefined) {
    this.#value = value;
    // Split on either separator: Umbraco writes "YYYY-MM-DD HH:mm:ss", but values saved by the v13
    // editor (or by code) may be plain ISO 8601 with a "T". An <input type="date"> shows nothing at
    // all if the value carries a time component, so the date part has to be isolated either way.
    this._inputValue = value ? value.split(/[ T]/)[0] : '';
  }
  public get value(): string | undefined {
    return this.#value;
  }
  #value?: string;

  @property({ type: Boolean, reflect: true })
  public readonly = false;

  @property({ type: Boolean })
  public mandatory = false;

  @state()
  private _inputValue = '';

  #onChange(event: Event & { target: HTMLInputElement }) {
    const date = event.target.value?.toString();
    this.value = date ? `${date} 00:00:00` : undefined;
    this.dispatchEvent(new UmbChangeEvent());
  }

  override render() {
    return html`
      <umb-input-date
        type="date"
        label=${this.localize.term('placeholders_enterdate')}
        .value=${this._inputValue}
        ?required=${this.mandatory}
        ?readonly=${this.readonly}
        @change=${this.#onChange}>
      </umb-input-date>
    `;
  }
}

export default LimboDatePropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-date-property-editor-ui': LimboDatePropertyEditorUiElement;
  }
}
