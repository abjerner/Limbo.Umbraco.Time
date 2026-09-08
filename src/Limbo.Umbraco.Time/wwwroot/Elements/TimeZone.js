import { html } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { UmbChangeEvent } from "@umbraco-cms/backoffice/event";

import { TimeService } from "@limbo/time/service";

const DEFAULT_TIME_ZONE = "local";

export class LimboTimeZonePropertyEditorUiElement extends UmbLitElement {

    static properties = {
        value: { type: String },
        readonly: { type: Boolean, reflect: true },
        _timeZones: { state: true },
        _error: { state: true },
        _loading: { state: true },
    };

    constructor() {
        super();
        this.value = undefined;
        this.readonly = false;
        this._timeZones = [];
        this._error = undefined;
        this._loading = true;
        void this.#loadTimeZones();
    }

    async #loadTimeZones() {
        try {
            const { data, error } = await TimeService.getTimeZones();

            if (error) throw error;

            this._timeZones = data ?? [];

            // NOTE: no default is written back here. Doing so used to dispatch an "UmbChangeEvent" as soon
            // as the data type workspace opened, marking it dirty before the user had touched anything (and
            // racing with Umbraco"s own assignment of "value"). An unset time zone already means "server
            // local" server side - see "GetTimeZoneInfo" in the value converters - so the select simply
            // falls back to DEFAULT_TIME_ZONE when rendering.
        } catch (error) {
            this._error = error instanceof Error ? error.message : "Failed to load time zones";
        } finally {
            // Always clear the loading flag - an empty response must not leave a spinner up forever.
            this._loading = false;
        }
    }

    #onChange(event) {
        this.value = event.target.value;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {

        if (this._loading) {
            return html`<uui-loader></uui-loader>`;
        }

        if (this._error) {
            return html`
                <uui-icon name="icon-alert"></uui-icon>
                ${this._error}
            `;
        }

        const selected = this.value ?? DEFAULT_TIME_ZONE;

        return html`
            <uui-select
                .value=${selected}
                ?disabled=${this.readonly}
                .options=${this._timeZones.map((timeZone) => ({
                    name: timeZone.name,
                    value: timeZone.id,
                    selected: timeZone.id === selected,
                }))}
                @change=${this.#onChange}>
            </uui-select>
        `;

    }

}

customElements.define("limbo-time-zone-property-editor-ui", LimboTimeZonePropertyEditorUiElement);

export default LimboTimeZonePropertyEditorUiElement;