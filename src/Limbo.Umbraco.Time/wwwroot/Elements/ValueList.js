// A plain string is stored - not an array as the built-in dropdown UI would - because the C# value
// converters switch directly on the configured string.
import { html } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

export class LimboTimeValueTypePropertyEditorUiElement extends UmbLitElement {

	static properties = {
		value: { type: String },
		readonly: { type: Boolean, reflect: true },
		config: { attribute: false },
		items: { state: true },
	};

	constructor() {
		super();
		this.value = undefined;
		this.readonly = false;
		this.items = [];
	}

	set config(config) {
		const items = config?.getValueByAlias("items");
		this.items = Array.isArray(items) ? items : [];
	}

	#onChange(event) {
		this.value = event.target.value;
		this.dispatchEvent(new UmbChangeEvent());
	}

	render() {
		const selected = this.value && this.items.includes(this.value) ? this.value : this.items[0];
		return html`
			<uui-select
				.value=${selected ?? ""}
				?disabled=${this.readonly}
				.options=${this.items.map((item) => ({
					name: item,
					value: item,
					selected: item === selected,
				}))}
				@change=${this.#onChange}>
			</uui-select>
		`;
	}
}

customElements.define("limbo-time-value-type-property-editor-ui", LimboTimeValueTypePropertyEditorUiElement);

export default LimboTimeValueTypePropertyEditorUiElement;