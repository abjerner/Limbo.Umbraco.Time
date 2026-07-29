// [CHANGE: upgrade to Umbraco 17] Related: index.ts, PropertyEditors/*/[Date|DateTime|Time]Configuration.cs
// Replaces the three AngularJS config views (DateValueType.html, DateTimeValueType.html,
// TimeValueType.html) with a single configuration-only property editor UI. The list of value types
// is passed per data type through the manifest ("config: [{ alias: 'items', value: [...] }]").
//
// A plain string is stored - not an array as the built-in dropdown UI would - because the C# value
// converters switch directly on the configured string.
import { customElement, html, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
  UmbPropertyEditorConfigCollection,
  UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';

@customElement('limbo-time-value-type-property-editor-ui')
export class LimboTimeValueTypePropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ type: String })
  public value?: string;

  @property({ type: Boolean, reflect: true })
  public readonly = false;

  @property({ attribute: false })
  public set config(config: UmbPropertyEditorConfigCollection | undefined) {
    const items = config?.getValueByAlias<Array<string>>('items');
    this._items = Array.isArray(items) ? items : [];
  }

  @state()
  private _items: Array<string> = [];

  #onChange(event: Event) {
    this.value = (event.target as HTMLSelectElement).value;
    this.dispatchEvent(new UmbChangeEvent());
  }

  override render() {
    const selected = this.value && this._items.includes(this.value) ? this.value : this._items[0];
    return html`
      <uui-select
        .value=${selected ?? ''}
        ?disabled=${this.readonly}
        .options=${this._items.map((item) => ({ name: item, value: item, selected: item === selected }))}
        @change=${this.#onChange}>
      </uui-select>
    `;
  }
}

export default LimboTimeValueTypePropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-time-value-type-property-editor-ui': LimboTimeValueTypePropertyEditorUiElement;
  }
}
