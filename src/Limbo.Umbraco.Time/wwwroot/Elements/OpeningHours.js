import { UmbElementMixin } from "@umbraco-cms/backoffice/element-api";
import { LitElement, html, css, repeat, when, nothing } from "@umbraco-cms/backoffice/external/lit";
import { UmbFormControlMixin } from '@umbraco-cms/backoffice/validation';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';

import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

import { hideLabel, parseBoolean, clone } from "@limbo/time/utils";


/** Monday first, matching OpeningHoursModel.StartOfWeek. The ID is the .NET DayOfWeek value. */
const WEEKDAYS = [
    { id: 1, alias: "monday" },
    { id: 2, alias: "tuesday" },
    { id: 3, alias: "wednesday" },
    { id: 4, alias: "thursday" },
    { id: 5, alias: "friday" },
    { id: 6, alias: "saturday" },
    { id: 0, alias: "sunday" }
];

export class LimboOpeningHoursElement extends UmbFormControlMixin(UmbLitElement, undefined) {

    static properties = {
        value: { attribute: false },
        readonly: { type: Boolean, reflect: true },
        config: { attribute: false },
        weekdays: { state: true },
        holidays: { state: true },
        hideWeekdays: { state: true },
        hideHolidays: { state: true },
        maxTimeSlots: { state: true }
    };

    get value() {
        const value = {};
        if (Object.keys(this.weekdays).length > 0) {
            value.weekdays = this.weekdays;
        }
        if (this.holidays.length > 0) {
            value.holidays = this.holidays;
        }
        return Object.keys(value).length > 0 ? value : undefined;
    }

    set value(value) {
        // Values saved by the v13 editor may arrive as a JSON string, or as the literal string "null".
        const parsed = typeof value === "string" ? this.#tryParse(value) : value;
        this.weekdays = clone(parsed?.weekdays ?? {});
        this.holidays = Array.isArray(parsed?.holidays) ? clone(parsed.holidays) : [];
    }

    set config(config) {
        // "parseBoolean" - not "Boolean" - because v13 data types stored these as "1"/"0" strings.
        this.hideWeekdays = parseBoolean(config?.getValueByAlias("hideWeekdays"));
        this.hideHolidays = parseBoolean(config?.getValueByAlias("hideHolidays"));
        const allowMultiple = parseBoolean(config?.getValueByAlias("allowMultipleTimeSlots"));
        this.maxTimeSlots = allowMultiple ? Number(config?.getValueByAlias("maxTimeSlots")) || 0 : 1;
        this.timeFormat = config?.getValueByAlias("timeFormat") ?? "HH:mm";
        this.step = this.timeFormat === "HH:mm:ss" ? 1 : 60;
        this.hideLabel = parseBoolean(config?.getValueByAlias("hideLabel"));
    }

    constructor() {
        super();
        this.readonly = false;
        this.weekdays = {};
        this.holidays = [];
        this.hideWeekdays = false;
        this.hideHolidays = false
        this.maxTimeSlots = 1;
    }

    connectedCallback() {
        super.connectedCallback();
        this.#validate();
        if (this.hideLabel) hideLabel(this);
    }

    #addValidationControls() {
        const inputs = this.shadowRoot.querySelectorAll("uui-input,umb-input-date");
        inputs.forEach(input => {
            this.addFormControlElement(input);
        });
    }

    #tryParse(value) {
        if (!value || value === "null") {
            return undefined;
        }
        try {
            return JSON.parse(value);
        } catch {
            return undefined;
        }
    }

    #validate() {
        this.#addValidationControls();
        const invalidHoliday = this.holidays.find((holiday) => !holiday.label?.trim() || !holiday.date);
        this.setCustomValidity(invalidHoliday ? 'Every holiday must have a label and a date.' : '');
    }

    #dispatch() {
        this.requestUpdate();
        this.dispatchEvent(new UmbChangeEvent());
    }

    #canAddTimeSlot(items) {
        return this.maxTimeSlots === 0 || items.length < this.maxTimeSlots;
    }

    #addWeekdaySlot(id) {

        const weekday = this.weekdays[id] ?? { items: [] };

        this.weekdays = {
            ...this.weekdays,
            [id]: {
                ...weekday,
                items: [ ...weekday.items, { opens: "09:00", closes: "17:00" } ]
            }
        };

        this.#validate();
        this.#dispatch();

    }

    #removeWeekdaySlot(id, index) {
        const weekday = this.weekdays[id];
        if (!weekday) { return; }
        weekday.items = weekday.items.filter((_, itemIndex) => itemIndex !== index,);
        if (weekday.items.length === 0) { delete this.weekdays[id]; }
        this.weekdays = { ...this.weekdays };
        this.#dispatch();
    }

    async #addHoliday() {
        this.holidays = [...this.holidays, { label: "", date: "", items: [], },];
        await this.updateComplete;
        this.#validate();
        this.#dispatch();
    }

    #removeHoliday(index) {
        this.holidays = this.holidays.filter((_, holidayIndex) => holidayIndex !== index,);
        this.#validate();
        this.#dispatch();
    }


    #addHolidaySlot(holiday) {
        holiday.items = [...holiday.items, { opens: "09:00", closes: "17:00", },];
        this.holidays = [...this.holidays];
        this.#validate();
        this.#dispatch();
    }

    #removeHolidaySlot(holiday, index) {
        holiday.items = holiday.items.filter((_, itemIndex) => itemIndex !== index,);
        this.holidays = [...this.holidays];
        this.#validate();
        this.#dispatch();
    }

    #onSlotChange(slot, key, event) {
        slot[key] = event.target.value;
        this.#validate();
        this.#dispatch();
    }

    #renderTimeSelect(slot, key) {
        return html`
            <uui-input
                type="time"
                label="Time"
                .value=${slot[key]}
                step="${this.step}"
                ?required=${!this.nullable}
                @input=${(event)=>this.#onSlotChange(slot, key, event)}
            >
            </uui-input>`;
    }

    #renderTimeSlots(items, onAdd, onRemove) {

        if (items.length === 0) {
            return html`
                <div class="closed"> ${this.localize.term("limboOpeningHours_closed")}.</div>
                <uui-button
                    compact
                    look="secondary"
                    ?disabled=${this.readonly}
                    label=${this.localize.term("limboOpeningHours_addOpeningHours")} @click=${onAdd}>
                    <uui-icon name="icon-add"></uui-icon>
                    ${this.localize.term("limboOpeningHours_addOpeningHours")}
                </uui-button>
            `;
        }

        return html`
            ${repeat(items, (_, index) => index, (slot, index) => html`
                <span class="slot">
                    ${index > 0 ? html` <span> ${this.localize.term("limboOpeningHours_and")} </span> ` : nothing}
                    <span> ${this.localize.term("limboOpeningHours_from")} </span>
                    ${this.#renderTimeSelect(slot, "opens")}
                    <span> ${this.localize.term("limboOpeningHours_to")} </span>
                    ${this.#renderTimeSelect(slot, "closes")}
                    <uui-button compact look="secondary" color="danger" ?disabled=${this.readonly} label="Remove" @click=${() => onRemove(index)}>
                        <uui-icon name="icon-delete"></uui-icon>
                    </uui-button>
                </span>
            `)}
            ${this.#canAddTimeSlot(items) ? html`
                <uui-button compact look="placeholder" ?disabled=${this.readonly} label="${this.localize.term("limboOpeningHours_addOpeningHours")}" @click=${onAdd}>
                    <uui-icon name="icon-add"></uui-icon>
                </uui-button>
            ` : nothing}
        `;

    }

    #renderWeekdays() {
        return html`
            <uui-box class="weekdays" headline=${this.localize.term("limboOpeningHours_weekdayTitle")}>
                <table>
                    <tbody>
                        ${repeat(WEEKDAYS, (day) => day.id, (day) => {
                            const items = this.weekdays[day.id]?.items ?? [];
                            return html`
                                <tr>
                                    <td class="label">
                                        ${this.localize.term(`limboOpeningHours_${day.alias}`)}
                                    </td>
                                    <td class="slots">
                                        <div>
                                            ${this.#renderTimeSlots(items, () => this.#addWeekdaySlot(day.id), (index) => this.#removeWeekdaySlot(day.id, index))}
                                        </div>
                                    </td>
                                </tr>
                            `;
                        })}
                    </tbody>
                </table>
            </uui-box>
        `;
    }

    #renderHolidays() {
        return html`
            <uui-box class="holidays" headline=${this.localize.term("limboOpeningHours_holidayTitle")}>
                <table>
                    <tbody>
                        ${repeat(this.holidays, (_, index) => index, (holiday, index) => html`
                            <tr>
                                <td class="label">
                                    <uui-input
                                        .value=${holiday.label}
                                        placeholder=${this.localize.term("limboOpeningHours_holidayLabelPlaceholder")}
                                        label=${this.localize.term("limboOpeningHours_holidayLabelPlaceholder")}
                                        required
                                        requiredMessage="Please enter a label"
                                        ?disabled=${this.readonly} @input=${(event) => { holiday.label = event.target.value; this.#dispatch(); }}
                                    >
                                    </uui-input>
                                </td>
                                <td>
                                    <umb-input-date
                                        type="date" label=${this.localize.term("limboOpeningHours_holidayDate")}
                                        .value=${holiday.date}
                                        ?readonly=${this.readonly}
                                        required
                                        requiredMessage="Please select a date"
                                        @change=${(event) => { holiday.date = event.target.value; this.#dispatch(); }}
                                    >
                                    </umb-input-date>
                                </td>
                                <td class="slots">
                                    <div>
                                        ${this.#renderTimeSlots(holiday.items, () => this.#addHolidaySlot(holiday), (slotIndex) => this.#removeHolidaySlot(holiday, slotIndex,))}
                                    </div>
                                </td>
                                <td>
                                    <uui-button compact look="secondary" color="danger" ?disabled=${this.readonly} label="Remove" @click=${() => this.#removeHoliday(index)}>
                                        <uui-icon name="icon-delete"></uui-icon>
                                    </uui-button>
                                </td>
                            </tr>
                        `,)}
                    </tbody>
                </table>
                <uui-button class="add-holiday" look="placeholder" ?disabled=${this.readonly} label=${this.localize.term("limboOpeningHours_addHoliday")} @click=${this.#addHoliday}>
                    <uui-icon name="icon-add"></uui-icon>
                    ${this.localize.term("limboOpeningHours_addHoliday")}
                </uui-button>
            </uui-box>
        `;
    }

    render() {
        return html`
            ${this.hideWeekdays ? nothing : this.#renderWeekdays()}
            ${this.hideHolidays ? nothing : this.#renderHolidays()}
            <pre>${JSON.stringify(this.value, null, 2)}</pre>
        `;
    }

    static styles = css`

        :host {
            display: flex; flex-direction: column;
            gap: 20px;
        }

        uui-box {
            --uui-box-default-padding: 15px;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        td {
            padding: var(--uui-size-space-2, 6px) var(--uui-size-space-2, 6px);
            vertical-align: top;
            border-bottom: 1px solid var(--uui-color-divider, #f3f3f5);
        }

        td.label {
            white-space: nowrap;
            font-weight: bold;
            min-width: 125px;
            padding-left: 0;
        }

        .weekdays td.label {
            line-height: 33px;
        }

        tr td:last-child {
            padding-right: 0;
        }

        .holidays .slots {
            width: 100%;
        }

        .slots > div {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        .slot { display: inline-flex; align-items: center; gap: var(--uui-size-space-2, 6px); margin-right: var(--uui-size-space-3, 9px); }

        .closed { margin-right: var(--uui-size-space-2, 6px); line-height: 33px; }

        select { padding: 4px; border: 1px solid var(--uui-color-border, #d8d7d9); border-radius: 3px; background-color: var(--uui-color-surface, #fff); color: inherit; }

        .add-holiday {
            margin-top: 10px;
            width: 100%;
        }

    `;

};

customElements.define("limbo-time-opening-hours", LimboOpeningHoursElement);

export default LimboOpeningHoursElement;