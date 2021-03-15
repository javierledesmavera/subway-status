import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json; charset=utf-8');
        headers = headers.set('Access-Control-Allow-Headers', '*');
        headers = headers.set('Access-Control-Allow-Methods', '*');
        headers = headers.set('Access-Control-Allow-Origins', '*');

        const apiReq = req.clone({ url: `${environment.webApiUrl}${req.url}`, headers: headers });
        return next.handle(apiReq);
    }
}