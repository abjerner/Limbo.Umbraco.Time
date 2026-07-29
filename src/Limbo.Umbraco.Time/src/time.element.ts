// [CHANGE: upgrade to Umbraco 17] Related: index.ts, PropertyEditors/Time/TimePropertyEditor.cs
// Replaces the AngularJS "TimePicker.html" view with its two number inputs (hour/minute). The value
// is still a plain "HH:mm" string, which TimeValueConverter parses via TimeSpan.TryParse.
import { customElement, html, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';

@customElement('limbo-time-property-editor-ui')
export class LimboTimePropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public set value(value: string | undefined) {
    this.#value = value;
    // Stored values may include seconds (e.g. "09:00:00"), but the input only handles "HH:mm".
    this._inputValue = value ? value.split(':').slice(0, 2).join(':') : '';
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
    const value = event.target.value?.toString();
    this.value = value ? value : undefined;
    this.dispatchEvent(new UmbChangeEvent());
  }

  override render() {
    return html`
      <umb-input-date
        type="time"
        label=${this.localize.term('placeholders_enterdate')}
        .value=${this._inputValue}
        ?required=${this.mandatory}
        ?readonly=${this.readonly}
        @change=${this.#onChange}>
      </umb-input-date>
    `;
  }
}

export default LimboTimePropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-time-property-editor-ui': LimboTimePropertyEditorUiElement;
  }
}
