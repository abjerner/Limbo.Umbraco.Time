// [CHANGE: upgrade to Umbraco 17] Related: index.ts, localization/en.ts, PropertyEditors/OpeningHours/OpeningHoursPropertyEditor.cs
// Replaces the AngularJS "OpeningHours.html" view and the "limboTimeWeekdays"/"limboTimeHolidays"/
// "limboTimeDatePicker" directives. The stored JSON shape is unchanged - weekdays keyed by
// DayOfWeek (0 = Sunday) and a holidays array - so OpeningHoursModel parses existing values as-is.
import { css, customElement, html, nothing, property, repeat, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import type {
  UmbPropertyEditorConfigCollection,
  UmbPropertyEditorUiElement,
} from '@umbraco-cms/backoffice/property-editor';
import { parseBoolean } from './parse-boolean.js';

interface LimboTimeSlot {
  opens: string;
  closes: string;
}

interface LimboWeekday {
  items: LimboTimeSlot[];
}

interface LimboHoliday {
  label: string;
  date: string;
  items: LimboTimeSlot[];
}

export interface LimboOpeningHoursValue {
  weekdays?: Record<string, LimboWeekday>;
  holidays?: LimboHoliday[];
}

/** Monday first, matching OpeningHoursModel.StartOfWeek. The ID is the .NET DayOfWeek value. */
const WEEKDAYS = [
  { id: 1, alias: 'monday' },
  { id: 2, alias: 'tuesday' },
  { id: 3, alias: 'wednesday' },
  { id: 4, alias: 'thursday' },
  { id: 5, alias: 'friday' },
  { id: 6, alias: 'saturday' },
  { id: 0, alias: 'sunday' },
];

/** The same five minute increments offered by the v13 directives. */
const TIMES: string[] = Array.from({ length: 288 }, (_, index) => {
  const hours = Math.floor(index / 12).toString().padStart(2, '0');
  const minutes = ((index % 12) * 5).toString().padStart(2, '0');
  return `${hours}:${minutes}`;
});

@customElement('limbo-opening-hours-property-editor-ui')
export class LimboOpeningHoursPropertyEditorUiElement extends UmbLitElement implements UmbPropertyEditorUiElement {

  @property({ attribute: false })
  public set value(value: LimboOpeningHoursValue | string | undefined) {
    // Values saved by the v13 editor may arrive as a JSON string, or as the literal string "null".
    const parsed = typeof value === 'string' ? this.#tryParse(value) : value;
    this._weekdays = parsed?.weekdays ?? {};
    this._holidays = Array.isArray(parsed?.holidays) ? parsed.holidays : [];
  }
  public get value(): LimboOpeningHoursValue | undefined {
    const value: LimboOpeningHoursValue = {};
    if (Object.keys(this._weekdays).length > 0) value.weekdays = this._weekdays;
    if (this._holidays.length > 0) value.holidays = this._holidays;
    return Object.keys(value).length > 0 ? value : undefined;
  }

  @property({ type: Boolean, reflect: true })
  public readonly = false;

  @property({ attribute: false })
  public set config(config: UmbPropertyEditorConfigCollection | undefined) {
    // "parseBoolean" - not "Boolean" - because v13 data types stored these as "1"/"0" strings.
    this._hideWeekdays = parseBoolean(config?.getValueByAlias('hideWeekdays'));
    this._hideHolidays = parseBoolean(config?.getValueByAlias('hideHolidays'));
    const allowMultiple = parseBoolean(config?.getValueByAlias('allowMultipleTimeSlots'));
    this._maxTimeSlots = allowMultiple ? Number(config?.getValueByAlias('maxTimeSlots')) || 0 : 1;
  }

  @state() private _weekdays: Record<string, LimboWeekday> = {};
  @state() private _holidays: LimboHoliday[] = [];
  @state() private _hideWeekdays = false;
  @state() private _hideHolidays = false;
  @state() private _maxTimeSlots = 1;

  #tryParse(value: string): LimboOpeningHoursValue | undefined {
    if (!value || value === 'null') return undefined;
    try {
      return JSON.parse(value) as LimboOpeningHoursValue;
    } catch {
      return undefined;
    }
  }

  #dispatch() {
    this.requestUpdate();
    this.dispatchEvent(new UmbChangeEvent());
  }

  #canAddTimeSlot(items: LimboTimeSlot[]) {
    return this._maxTimeSlots === 0 || items.length < this._maxTimeSlots;
  }

  #addWeekdaySlot(id: number) {
    const weekday = (this._weekdays[id] ??= { items: [] });
    weekday.items = [...weekday.items, { opens: '09:00', closes: '17:00' }];
    this._weekdays = { ...this._weekdays };
    this.#dispatch();
  }

  #removeWeekdaySlot(id: number, index: number) {
    const weekday = this._weekdays[id];
    if (!weekday) return;
    weekday.items = weekday.items.filter((_, i) => i !== index);
    if (weekday.items.length === 0) delete this._weekdays[id];
    this._weekdays = { ...this._weekdays };
    this.#dispatch();
  }

  #addHoliday() {
    this._holidays = [...this._holidays, { label: '', date: '', items: [] }];
    this.#dispatch();
  }

  #removeHoliday(index: number) {
    this._holidays = this._holidays.filter((_, i) => i !== index);
    this.#dispatch();
  }

  #addHolidaySlot(holiday: LimboHoliday) {
    holiday.items = [...holiday.items, { opens: '09:00', closes: '17:00' }];
    this._holidays = [...this._holidays];
    this.#dispatch();
  }

  #removeHolidaySlot(holiday: LimboHoliday, index: number) {
    holiday.items = holiday.items.filter((_, i) => i !== index);
    this._holidays = [...this._holidays];
    this.#dispatch();
  }

  #onSlotChange(slot: LimboTimeSlot, key: keyof LimboTimeSlot, event: Event) {
    slot[key] = (event.target as HTMLSelectElement).value;
    this.#dispatch();
  }

  #renderTimeSelect(slot: LimboTimeSlot, key: keyof LimboTimeSlot) {
    return html`
      <select
        .value=${slot[key]}
        ?disabled=${this.readonly}
        @change=${(event: Event) => this.#onSlotChange(slot, key, event)}>
        ${repeat(
          TIMES,
          (time) => time,
          (time) => html`<option value=${time} ?selected=${time === slot[key]}>${time}</option>`,
        )}
      </select>
    `;
  }

  #renderTimeSlots(items: LimboTimeSlot[], onAdd: () => void, onRemove: (index: number) => void) {
    if (items.length === 0) {
      return html`
        <span class="closed">${this.localize.term('limboOpeningHours_closed')}.</span>
        <uui-button
          compact
          look="secondary"
          ?disabled=${this.readonly}
          label=${this.localize.term('limboOpeningHours_addOpeningHours')}
          @click=${onAdd}>
          <uui-icon name="icon-add"></uui-icon>
          ${this.localize.term('limboOpeningHours_addOpeningHours')}
        </uui-button>
      `;
    }

    return html`
      ${repeat(
        items,
        (_, index) => index,
        (slot, index) => html`
          <span class="slot">
            ${index > 0 ? html`<span>${this.localize.term('limboOpeningHours_and')}</span>` : nothing}
            <span>${this.localize.term('limboOpeningHours_from')}</span>
            ${this.#renderTimeSelect(slot, 'opens')}
            <span>${this.localize.term('limboOpeningHours_to')}</span>
            ${this.#renderTimeSelect(slot, 'closes')}
            <uui-button
              compact
              look="secondary"
              ?disabled=${this.readonly}
              label="Remove"
              @click=${() => onRemove(index)}>
              <uui-icon name="icon-delete"></uui-icon>
            </uui-button>
          </span>
        `,
      )}
      ${this.#canAddTimeSlot(items)
        ? html`
            <uui-button compact look="secondary" ?disabled=${this.readonly} label="Add" @click=${onAdd}>
              <uui-icon name="icon-add"></uui-icon>
            </uui-button>
          `
        : nothing}
    `;
  }

  #renderWeekdays() {
    return html`
      <uui-box headline=${this.localize.term('limboOpeningHours_weekdayTitle')}>
        <table>
          <tbody>
            ${repeat(
              WEEKDAYS,
              (day) => day.id,
              (day) => {
                const items = this._weekdays[day.id]?.items ?? [];
                return html`
                  <tr>
                    <td class="label">${this.localize.term(`limboOpeningHours_${day.alias}`)}</td>
                    <td>
                      ${this.#renderTimeSlots(
                        items,
                        () => this.#addWeekdaySlot(day.id),
                        (index) => this.#removeWeekdaySlot(day.id, index),
                      )}
                    </td>
                  </tr>
                `;
              },
            )}
          </tbody>
        </table>
      </uui-box>
    `;
  }

  #renderHolidays() {
    return html`
      <uui-box headline=${this.localize.term('limboOpeningHours_holidayTitle')}>
        <table>
          <tbody>
            ${repeat(
              this._holidays,
              (_, index) => index,
              (holiday, index) => html`
                <tr>
                  <td class="label">
                    <uui-input
                      .value=${holiday.label}
                      placeholder=${this.localize.term('limboOpeningHours_holidayLabelPlaceholder')}
                      label=${this.localize.term('limboOpeningHours_holidayLabelPlaceholder')}
                      ?disabled=${this.readonly}
                      @input=${(event: Event) => {
                        holiday.label = (event.target as HTMLInputElement).value;
                        this.#dispatch();
                      }}>
                    </uui-input>
                  </td>
                  <td>
                    <umb-input-date
                      type="date"
                      label=${this.localize.term('limboOpeningHours_holidayDate')}
                      .value=${holiday.date}
                      ?readonly=${this.readonly}
                      @change=${(event: Event) => {
                        holiday.date = (event.target as HTMLInputElement).value;
                        this.#dispatch();
                      }}>
                    </umb-input-date>
                  </td>
                  <td>
                    ${this.#renderTimeSlots(
                      holiday.items,
                      () => this.#addHolidaySlot(holiday),
                      (slotIndex) => this.#removeHolidaySlot(holiday, slotIndex),
                    )}
                  </td>
                  <td>
                    <uui-button
                      compact
                      look="secondary"
                      color="danger"
                      ?disabled=${this.readonly}
                      label="Remove"
                      @click=${() => this.#removeHoliday(index)}>
                      <uui-icon name="icon-delete"></uui-icon>
                    </uui-button>
                  </td>
                </tr>
              `,
            )}
          </tbody>
        </table>
        <uui-button
          look="placeholder"
          ?disabled=${this.readonly}
          label=${this.localize.term('limboOpeningHours_addHoliday')}
          @click=${this.#addHoliday}>
          <uui-icon name="icon-add"></uui-icon>
          ${this.localize.term('limboOpeningHours_addHoliday')}
        </uui-button>
      </uui-box>
    `;
  }

  override render() {
    return html`
      ${this._hideWeekdays ? nothing : this.#renderWeekdays()}
      ${this._hideHolidays ? nothing : this.#renderHolidays()}
    `;
  }

  static override styles = [
    css`
      :host {
        display: flex;
        flex-direction: column;
        gap: var(--uui-size-space-4, 12px);
      }
      table {
        width: 100%;
        border-collapse: collapse;
      }
      td {
        padding: var(--uui-size-space-2, 6px) var(--uui-size-space-2, 6px);
        vertical-align: middle;
        border-bottom: 1px solid var(--uui-color-divider, #f3f3f5);
      }
      td.label {
        white-space: nowrap;
        font-weight: bold;
      }
      .slot {
        display: inline-flex;
        align-items: center;
        gap: var(--uui-size-space-2, 6px);
        margin-right: var(--uui-size-space-3, 9px);
      }
      .closed {
        margin-right: var(--uui-size-space-2, 6px);
      }
      select {
        padding: 4px;
        border: 1px solid var(--uui-color-border, #d8d7d9);
        border-radius: 3px;
        background-color: var(--uui-color-surface, #fff);
        color: inherit;
      }
    `,
  ];
}

export default LimboOpeningHoursPropertyEditorUiElement;

declare global {
  interface HTMLElementTagNameMap {
    'limbo-opening-hours-property-editor-ui': LimboOpeningHoursPropertyEditorUiElement;
  }
}
