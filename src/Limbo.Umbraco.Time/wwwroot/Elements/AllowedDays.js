import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";

import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

import { CalendarService } from "@limbo/time/service";

export class LimboOpeningHoursAllowedDaysElement extends UmbElementMixin(LitElement) {

    constructor() {
        super();
    }

    connectedCallback() {

        super.connectedCallback();

        const selected = Array.isArray(this.value) ? this.value : [];

        this.categories = CalendarService.getCategories();

        this.categories.forEach(function(c) {
            c.days.forEach(function(d) {
                d.selected = selected.indexOf(d.alias) !== -1
            })
        });

    }

    #onToggle(day) {

        day.selected = !day.selected;

        const temp = [];

        this.categories.forEach(function(c) {
            c.days.forEach(function(d) {
                if (d.selected) temp.push(d.alias);
            })
        });

        this.value = temp;

        this.requestUpdate();
        this.dispatchEvent(new UmbChangeEvent());

    }

    #formatDate(value) {
        if (!value) return "";
        return this.localize.date(value, { dateStyle: "full" });
    }

    render() {
        return html`
            ${repeat(this.categories, (category) => html`
                <uui-box headline="${this.localize.term("limboOpeningHours_category_" + category.alias)}">
                    <ul>
                        ${repeat(category.days, (day) => html`
                            <li>
                                <div>
                                    <uui-checkbox .checked=${day.selected} @change=${() => this.#onToggle(day)}>
                                        <span class="day-name">${this.localize.term("limboOpeningHours_day_" + day.alias)}</span>
                                        <small class="day-date">(${this.#formatDate(day.nextDate)})</small>
                                    </uui-checkbox>
                                </div>
                            </li>
                        `)}
                    </ul>
                </uui-box>
            `)}
            ${when(false, () => html`
                <pre>${JSON.stringify(this.categories, null, 2)}</pre>
                <pre>${JSON.stringify(this.value, null, 2)}</pre>
            `)}
        `;
    }

    static styles = css`

        uui-box + uui-box {
            margin-top: 20px;
        }

        ul, li {
            list-style: none;
            margin: 0;
            padding: 0;
        }

        li > div{
            display: flex;
            gap: 10px;
        }

        span {
            font-weight: bold;
        }

        small {
            color: #666;
        }

    `;

};

customElements.define("limbo-time-opening-hours-allowed-days", LimboOpeningHoursAllowedDaysElement);

export default LimboOpeningHoursAllowedDaysElement;