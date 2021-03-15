import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Alert } from 'src/app/models/alert';
import { Line } from 'src/app/models/line';
import { Stop } from 'src/app/models/stop';
import { SubwayService } from 'src/app/services/subway.service';

@Component({
  selector: 'app-arrivals',
  templateUrl: './arrivals.component.html',
  styleUrls: ['./arrivals.component.scss']
})
export class ArrivalsComponent implements OnInit {

  alerts: Array<Alert>;
  lines$: Observable<Array<Line>>;
  stops$: Observable<Array<Stop>>;
  stopHeaders$: Observable<Array<Stop>>;
  selectedLineId: string;
  selectedStopId: string;
  selectedStopHeadId: string;
  arrivalTime: Date;

  constructor(private service: SubwayService) { }

  ngOnInit() {
    this.lines$ = this.service.getLines();
  }

  onLinesChange(lineId: string) {
    this.selectedLineId = lineId;
    this.stops$ = this.service.getStopsByLineId(lineId);
    
  }

  onStopChange(stopId: string) {
    this.selectedStopId = stopId;
    this.stopHeaders$ = this.service.getStopHeadersByLineIdAndStopId(this.selectedLineId, stopId);
  }

  onStopHeaderChange(stopHeaderId: string) {
    this.selectedStopHeadId = stopHeaderId;
  }

  getNextArrival() {
    this.service.getNextArrivalToStop(this.selectedLineId, this.selectedStopId, this.selectedStopHeadId).subscribe((response: Date) => {
      this.arrivalTime = response;
    }, error => console.log(error));
  }

}
