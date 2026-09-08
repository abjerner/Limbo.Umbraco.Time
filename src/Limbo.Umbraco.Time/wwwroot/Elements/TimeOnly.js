import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";

import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

export class LimboTimeTimeOnlyElement extends UmbElementMixin(LitElement) {

    static properties = {
        value: { type: String },
        config: { type: Object }
    };

    get nullable() {
        return this.config?.getValueByAlias?.("nullable") === true;
    }

    get timeFormat() {
        return this.config?.getValueByAlias?.("timeFormat");
    }

    get step() {
        return this.timeFormat === "HH:mm:ss" ? 1 : 60;
    }

    constructor() {
        super();
        this.value = "";
        this.config = undefined;
    }

    connectedCallback() {
        super.connectedCallback();
        this.inputValue = this.value;
    }

    onInput(event) {
        const inputValue = event.target.value;
        this.value = inputValue ? inputValue : null;
        this.dispatchEvent(new UmbChangeEvent());
    }

    reset() {
        this.value = null;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {
        const hasValue = this.value !== null && this.value !== undefined && this.value !== "";
        return html`
            <div class="time-only-editor">
                <uui-input type="time" label="Time" .value=${this.inputValue} step="${this.step}" ?required=${!this.nullable} @input=${this.onInput}></uui-input>
                ${when(this.nullable && hasValue, () => html`
                    <uui-button look="secondary" label="Clear time" @click=${this.reset}>Clear</uui-button>
                `)}
            </div>
        `;
    }

    static styles = css`

        .time-only-editor {
            display: flex;
            align-items: center;
            gap: var(--uui-size-space-3);
        }

        uui-input {
            width: 160px;
        }

    `;

};

customElements.define("limbo-time-timeonly", LimboTimeTimeOnlyElement);

export default LimboTimeTimeOnlyElement;