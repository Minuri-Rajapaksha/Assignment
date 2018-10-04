import { Injectable } from '@angular/core';
import { RequestOptions, Headers } from '@angular/http';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/internal/operators/catchError';
import { of, throwError, Observable } from 'rxjs';
import { AuthService } from './auth-service';
import { HttpErrorResponse, HttpClient, HttpRequest, HttpParams, HttpEvent } from '@angular/common/http';
import Api from '../services/api-config.json';

declare let apiBaseUrl: string;
declare let faroUrl: string;

@Injectable()
export class ApiService {

    private _baseUrl: string;

    constructor(private http: HttpClient, private authService: AuthService) {
        this._baseUrl = Api.BASE_URL;
    }

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            console.error('An error occurred:', error.error.message);
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            console.error(
                `Backend returned code ${error.status}, ` +
                `body was: ${error.error}`);
        }
        // return an observable with a user-facing error message
        return throwError('Something bad happened; please try again later.');
    }

    private extractData(res: Response) {
        const body = res;
        return body || {};
    }

    get(url: string): Observable<any> {
        const uri = this._baseUrl + url;
        return this.http
            .get(uri, this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }

    getCustom(url: string): Observable<any> {
        const uri = this._baseUrl + url;
        return this.http
            .get(uri, this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }

    post(url: string, data?: any): Observable<any> {
        return this.http.post(this._baseUrl + url, data, this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }

    uploadFile(url: string, formData?: FormData): Observable<any> {
        const uploadReq = new HttpRequest('POST', this._baseUrl + url, formData, {
            reportProgress: true
        });
        return this.http.request(uploadReq)
            .pipe(map(this.extractData), catchError(this.handleError));
    }

    public upload(url: string, file: File): Observable<HttpEvent<any>> {
        let formData = new FormData();
        formData.append('upload', file);
        let params = new HttpParams();
        const options = {
          params: params,
          reportProgress: true,
          headers: this.authService.getAuthorizationHeaderValue()
        };
        const req = new HttpRequest('POST', url, formData, options);
        return this.http.request(req);
      }

    delete(url: string, id: string): Observable<any> {
        return this.http.delete(this._baseUrl + url + '/' + id, this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }

    put(url: string, data?: any): Observable<any> {
        return this.http.put(this._baseUrl + url, JSON.stringify(data), this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }
}
