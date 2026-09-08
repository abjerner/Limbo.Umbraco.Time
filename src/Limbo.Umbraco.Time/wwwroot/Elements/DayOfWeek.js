import { LimboTimeButtonListElement } from "@limbo/time/elements/button-list";

export class LimboTimeDayOfWeekElement extends LimboTimeButtonListElement {

    constructor() {

        super();

        this.options = [
            { alias: "monday", name: this.localize.term("limboTime_monday") },
            { alias: "tuesday", name: this.localize.term("limboTime_tuesday") },
            { alias: "wednesday", name: this.localize.term("limboTime_wednesday") },
            { alias: "thursday", name: this.localize.term("limboTime_thursday") },
            { alias: "friday", name: this.localize.term("limboTime_friday") },
            { alias: "saturday", name: this.localize.term("limboTime_saturday") },
            { alias: "sunday", name: this.localize.term("limboTime_sunday") }
        ];

    }

};

customElements.define("limbo-time-day-of-week", LimboTimeDayOfWeekElement);

export default LimboTimeDayOfWeekElement;