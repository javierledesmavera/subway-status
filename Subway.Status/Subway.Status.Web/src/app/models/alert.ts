import { BaseModel } from "./base-model";

export class Alert extends BaseModel {
    routeId: string;
    stopId: string;
    headerText: string;
    descriptionText: string;
    cause: number;
    effect: number;
    reportedDate: Date;
}