import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ArrivalsComponent } from './components/arrivals/arrivals.component';
import { HistoryComponent } from './components/history/history.component';
import { StatusComponent } from './components/status/status.component';

const routes: Routes = [
  {
    path: 'status',
    component: StatusComponent
  },
  {
    path: 'arrivals',
    component: ArrivalsComponent
  },
  {
    path: 'history',
    component: HistoryComponent
  },
  {
    path: '',
    redirectTo: '/status',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/status',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
