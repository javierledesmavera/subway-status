import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Alert } from '../models/alert';
import { Line } from '../models/line';
import { ServiceAlert } from '../models/service-alert';
import { Stop } from '../models/stop';

@Injectable({
  providedIn: 'root'
})
export class SubwayService {

  private baseUrl: string = "subway/";

  constructor(private http: HttpClient) { }

  public getAlerts() : Observable<Array<Alert>> {
    return this.http.get<ServiceAlert>(this.baseUrl + 'serviceAlerts').pipe(map((serviceAlert: ServiceAlert) => {
      return serviceAlert.alerts;
    }));
  }

  public getLines() : Observable<Array<Line>> {
    return this.http.get<Array<Line>>(this.baseUrl + 'lines');
  }

  public getStopsByLineId(lineId: string) : Observable<Array<Stop>> {
    return this.http.get<Array<Stop>>(this.baseUrl + `stops/${lineId}`);
  }

  public getStopHeadersByLineIdAndStopId(lineId: string, stopId: string) : Observable<Array<Stop>> {
    return this.http.get<Array<Stop>>(this.baseUrl + `stops/${lineId}/headers/${stopId}`);
  }

  public getNextArrivalToStop(lineId: string, stopId: string, destinationStopId: string) : Observable<Date> {
    return this.http.get<Date>(this.baseUrl + `arrivals/${lineId}/stops/${stopId}?destinationStopId=${destinationStopId}`);
  }

  public getAlertsFiltered(lineId: string, request: any) : Observable<Array<Alert>> {
    return this.http.post<Array<Alert>>(this.baseUrl + `alerts/${lineId}`, request);
  }
}
