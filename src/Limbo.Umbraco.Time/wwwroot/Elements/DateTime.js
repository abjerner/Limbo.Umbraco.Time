import { html } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

import { parseBoolean, utcToLocal } from "@limbo/time/utils";

export class LimboDateTimePropertyEditorUiElement extends UmbLitElement {

    static properties = {
        value: { type: String },
        readonly: { type: Boolean, reflect: true },
        mandatory: { type: Boolean },
        config: { attribute: false },
        _configReadonly: { state: true },
        _inputValue: { state: true },
    };

    #value;

    constructor() {
        super();

        this.readonly = false;
        this.mandatory = false;
        this._configReadonly = false;
        this._inputValue = "";
    }

    set value(value) {

        const oldValue = this.#value;

        this.#value = value;

        // The value is saved as UTC, but without any mention of the time zone, so we need to
        // convert it to local time for the UI
        this._inputValue = value ? utcToLocal(value) : "";

        this.requestUpdate("value", oldValue);

    }

    get value() {
        return this.#value;
    }

    set config(config) {
        // "parseBoolean" - not truthiness - because v13 data types stored this as a "1"/"0" string.
        // Kept separate from "readonly" so that assigning the config never clears the readonly state
        // Umbraco itself sets on the element.
        this._configReadonly = parseBoolean(
            config?.getValueByAlias("readonly")
        );
    }

    #onChange(event) {
        this.value = event.target.value ? new Date(event.target.value).toISOString().replace("T", " ").split(".")[0] : undefined;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {
        return html`
            <umb-input-date
                type="datetime-local"
                step="1"
                label=${this.localize.term("placeholders_enterdate")}
                .value=${this._inputValue}
                ?required=${this.mandatory}
                ?readonly=${this.readonly || this._configReadonly}
                @change=${this.#onChange}>
            </umb-input-date>
        `;
    }
}

customElements.define("limbo-datetime-property-editor-ui", LimboDateTimePropertyEditorUiElement);

export default LimboDateTimePropertyEditorUiElement;