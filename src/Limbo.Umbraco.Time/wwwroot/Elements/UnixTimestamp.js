import { css, html, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

import { parseBoolean } from "@limbo/time/utils";

/** Formats a Date as the "YYYY-MM-DDTHH:mm:ss" string expected by a datetime-local input. */
function toInputValue(date) {
    const pad = (value) => value.toString().padStart(2, "0");
    return (
        `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}` +
        `T${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`
    );
}

export class LimboUnixTimestampPropertyEditorUiElement extends UmbLitElement {

    static properties = {
        value: { type: String },
        readonly: { type: Boolean, reflect: true },
        mandatory: { type: Boolean },
        config: { attribute: false },
        _configReadonly: { state: true },
        _inputValue: { state: true },
        _showUnixTimestamp: { state: true },
    };

    #value;

    constructor() {
        super();
        this.readonly = false;
        this.mandatory = false;
        this._configReadonly = false;
        this._inputValue = "";
        this._showUnixTimestamp = false;
    }

    set value(value) {
        const oldValue = this.#value;
        this.#value = value;
        const seconds = Number(value);
        this._inputValue = value && !isNaN(seconds) && seconds !== 0 ? toInputValue(new Date(seconds * 1000)) : "";
        this.requestUpdate("value", oldValue);
    }

    get value() {
        return this.#value;
    }

    set config(config) {
        // "parseBoolean" - not truthiness/"Boolean" - because v13 data types stored these as "1"/"0"
        // strings. "_configReadonly" is kept separate from "readonly" so that assigning the config never
        // clears the readonly state Umbraco itself sets on the element.
        this._configReadonly = parseBoolean(config?.getValueByAlias("readonly"));
        this._showUnixTimestamp = parseBoolean(config?.getValueByAlias("showUnixTimestamp"));
    }

    #onChange(event) {
        const value = event.target.value?.toString();
        const date = value ? new Date(value) : undefined;
        this.value = date && !isNaN(date.getTime()) ? Math.floor(date.getTime() / 1000).toString() : undefined;
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
            ${when(this._showUnixTimestamp && this.value, () => html`
                <div class="timestamp">
                <strong>Unix timestamp:</strong> ${this.value}
                </div>
            `)}
        `;
    }

    static styles = css`

        :host {
            display: block;
        }

        .timestamp {
            margin-top: var(--uui-size-space-2, 6px);
            font-size: 0.85em;
        }

    `;

}

customElements.define("limbo-unix-timestamp-property-editor-ui", LimboUnixTimestampPropertyEditorUiElement);

export default LimboUnixTimestampPropertyEditorUiElement;