import { LitElement, html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

export class LimboDatePropertyEditorUiElement extends UmbElementMixin(LitElement) {

    static properties = {
        value: { type: String },
        readonly: { type: Boolean, reflect: true },
        mandatory: { type: Boolean },
        inputValue: { state: true },
    };

    #value;

    set value(value) {
        const oldValue = this.#value;
        this.#value = value;
        // Split on either separator: Umbraco writes "YYYY-MM-DD HH:mm:ss", but values saved by the v13
        // editor (or by code) may be plain ISO 8601 with a "T". An <input type="date"> shows nothing at
        // all if the value carries a time component, so the date part has to be isolated either way.
        this.inputValue = value ? value.split(/[ T]/)[0] : "";
        this.requestUpdate("value", oldValue);
    }

    get value() {
        return this.#value;
    }

    constructor() {
        super();
        this.readonly = false;
        this.mandatory = false;
        this.inputValue = "";
    }

    #onChange(event) {
        const date = event.target.value?.toString();
        this.value = date ? date : undefined;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {
        return html`
            <umb-input-date
                type="date"
                label=${this.localize.term('placeholders_enterdate')}
                .value=${this.inputValue}
                ?required=${this.mandatory}
                ?readonly=${this.readonly}
                @change=${this.#onChange}>
            </umb-input-date>
            <pre>${this.value}</pre>
            <pre>${this.inputValue}</pre>
        `;
    }

};


customElements.define("limbo-date-property-editor-ui", LimboDatePropertyEditorUiElement);

export default LimboDatePropertyEditorUiElement;