import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { catchError } from 'rxjs/internal/operators/catchError';
import { throwError, Observable } from 'rxjs';
import { AuthService } from './auth-service';
import { HttpErrorResponse, HttpClient, HttpRequest, HttpParams, HttpEvent } from '@angular/common/http';

@Injectable()
export class ApiService {

    private _baseUrl: string;

    constructor(private http: HttpClient, private authService: AuthService) {
        if (window.location.hostname === 'localhost') {
            this._baseUrl = `https://localhost:44310/api/`;
        } else {
            this._baseUrl = `https://webapiminuri.azurewebsites.net/api/`;
        }
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

    post(url: string, data?: any): Observable<any> {
        return this.http.post(this._baseUrl + url, data, this.authService.getAuthorizationHeaderValue())
            .pipe(map(this.extractData), catchError(this.handleError));
    }
}
