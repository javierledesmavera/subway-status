import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { Alert } from 'src/app/models/alert';
import { Line } from 'src/app/models/line';
import { SubwayService } from 'src/app/services/subway.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

  alerts: Array<Alert>;
  lines$: Observable<Array<Line>>;
  searchForm: FormGroup;
  submitted: boolean = false;

  constructor(private service: SubwayService, private fb: FormBuilder, private calendar: NgbCalendar, public formatter: NgbDateParserFormatter) { }

  ngOnInit() {
    this.lines$ = this.service.getLines();

    this.searchForm = this.fb.group({
      lineId: ['', [Validators.required]],
      fromDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]]
    });
  }

  cleanFilters() {
    this.alerts = null;
    this.searchForm.reset();
    this.searchForm.updateValueAndValidity();
  }

  getAlertsFiltered() {
    this.submitted = true;
    if (!this.searchForm.valid) {
      return;
    }

    var request = JSON.parse(JSON.stringify(this.searchForm.value));
    request.toDate = this.convertNgbDateToDate(request.toDate);
    request.fromDate = this.convertNgbDateToDate(request.fromDate);
    this.service.getAlertsFiltered(request.lineId, request).subscribe(alerts => {
      this.alerts = alerts;
      this.submitted = false;
    });
  }

  convertNgbDateToDate(ngbDate: NgbDate) : Date {
    return new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);
  }

}
