import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when } from "@umbraco-cms/backoffice/external/lit";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

export class LimboTimeButtonListElement extends UmbElementMixin(LitElement) {

    static properties = {
        value: { type: String }
    };

    #config = { nullable: false };

    set config(config) {
        if (!config) return;
        this.#config = {
            nullable: config.getValueByAlias("nullable") === true
        };
        this.requestUpdate();
    }

    constructor() {

        super();

        this.value = "";

        this.options = [];

    }

    select(option) {
        this.value = option ? option.alias : null;
        this.dispatchEvent(new UmbChangeEvent());
        this.requestUpdate();
    }

    getClassList(option) {
        const temp = [];
        if (option && option.alias == this.value) temp.push("--active");
        return temp;
    }

    render() {

        return html`
            <div class="options">
                ${repeat(this.options, option => option.alias, option => html`
                    <uui-button class="${this.getClassList(option).join(" ")}" @click=${() => this.select(option)} label=${option.name}>
                        ${option.name}
                    </button>
                `)}
                ${when(this.#config.nullable, () => html`
                    <uui-button class="${!this.value ? "--active" : ""}" @click=${() => this.select(null)} label=${this.localize.term("limboTime_none")}>
                        ${this.localize.term("limboTime_none")}
                    </uui-button>
                `)}
            </div>
            <pre>${JSON.stringify(this.#config, null, 2)}</pre>
        `;

    }

    static styles = css`

        div.options {
            display: flex;
            flex-wrap: wrap;
            gap: 7px;
        }

        uui-button {
            --uui-button-background-color: rgba(216,215,217, .5);
            --uui-button-background-color-hover: rgba(216,215,217, .3);
            --uui-button-font-weight: bold;
            --uui-button-font-size: 13px;
        }

        uui-button.--active {
            --uui-button-background-color: #F5C1BC;
            --uui-button-background-color-hover: #F5C1BC;
        }

        button {
            appearance: none;
            display: inline-block;
            -webkit-appearance: none;
            border: 0;
            font-weight: bold;
            color: #1A2650;
            line-height: 1;
            background-color: rgba(216,215,217, .5);
            font-size: 13px;
            padding: 10px 20px;
            border-radius: 4px;
            transition: all .2s ease;
            position: relative;
            cursor: pointer;
            &:hover {
                background-color: rgba(216,215,217, .3);
                color: #2152a3;
            }
            &.--active {
                background-color: #F5C1BC;
                color: #1A2650;
            }
        }

    `;

}

export default LimboTimeButtonListElement;