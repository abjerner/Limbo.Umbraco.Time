// [CHANGE: upgrade to Umbraco 17] Related: date.element.ts, index.ts, PropertyEditors/DateTime/DateTimePropertyEditor.cs
// Replaces the AngularJS "DateTime.html" view + "Limbo.Umbraco.DateTime.Controller". The stored
// format ("YYYY-MM-DD HH:mm:ss") matches Umbraco's own DateTime data editor.
//
// Behavioural note: the v13 controller shifted the picked date by the browser's UTC offset before
// saving. This element saves the wall clock time as picked, exactly like the built-in Umbraco date
// picker does in v14+. The time zone configured on the data type is still applied server side by
// DateTimeValueConverter.
import { customElement, html, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
  UmbPropertyEditorConfigCollection,
  UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { parseBoolean } from './parse-boolean.js';

@customElement('limbo-datetime-property-editor-ui')
export class LimboDateTimePropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public set value(value: string | undefined) {
    this.#value = value;
    this._inputValue = value ? value.replace(' ', 'T') : '';
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
    // "parseBoolean" - not truthiness - because v13 data types stored this as a "1"/"0" string.
    // Kept separate from "readonly" so that assigning the config never clears the readonly state
    // Umbraco itself sets on the element.
    this._configReadonly = parseBoolean(config?.getValueByAlias('readonly'));
  }

  @state()
  private _configReadonly = false;

  @state()
  private _inputValue = '';

  #onChange(event: Event & { target: HTMLInputElement }) {
    const value = event.target.value?.toString();
    this.value = value ? `${value.replace('T', ' ')}${value.length === 16 ? ':00' : ''}` : undefined;
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
    `;
  }
}

export default LimboDateTimePropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-datetime-property-editor-ui': LimboDateTimePropertyEditorUiElement;
  }
}
