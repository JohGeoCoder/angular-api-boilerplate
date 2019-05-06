import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Application } from 'src/app/models/application';
import { Observable } from 'rxjs/internal/Observable';

import { map, find } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {

  private _applications: BehaviorSubject<Application[]> = new BehaviorSubject([]);

  constructor(private http: HttpClient) { 
    this.loadInitialData();
  }

  get applications(): Observable<Application[]> {
    return new Observable(fn => this._applications.subscribe(fn));
  }

  getApplicationById(id: number): Observable<Application> {
    return new Observable(fn => 
      this._applications.subscribe((applications: Application[]) => 
        fn.next(applications.find((application: Application) => application.id === id))));
  }

  createApplication = (application: Application): void => {
    this.http.post('https://localhost:44337/api/applications', application)
      .subscribe((application: Application): void => {
        if(application){
          this.loadInitialData();
        }
      })
  }

  updateApplication(application: Application): Observable<Application> {
    return new Observable();
  }

  deleteApplication(application: Application): Observable<Application> {
    return new Observable();
  }

  private loadInitialData(){
    this.http.get('https://localhost:44337/api/applications')
      .pipe(
        map((applications: Application[]) => {
          return applications.map(app => <Application> app )
        })
      )
      .subscribe(
        (res) => {
          this._applications.next(res);
        },
        err => console.log("Error retrieving applications")
      )
  }
}
