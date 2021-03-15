import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Alert } from 'src/app/models/alert';
import { SubwayService } from 'src/app/services/subway.service';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
  styleUrls: ['./status.component.scss']
})
export class StatusComponent implements OnInit {

  alerts$: Observable<Array<Alert>>;

  constructor(private service: SubwayService) { }

  ngOnInit() {
    this.alerts$ = this.service.getAlerts();
  }

}
