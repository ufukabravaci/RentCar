/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-non-null-assertion */
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ErrorService } from './error';
import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  readonly #http = inject(HttpClient);
  readonly #err = inject(ErrorService);

  get<T>(
    endoint: string,
    callBack: (res: T) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {
    this.#http.get<Result<T>>(endoint).subscribe({
      next: (res) => {
        callBack(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#err.handle(err);
        if (errorCallBack) {
          errorCallBack(err);
        }
      },
    });
  }

  post<T>(
    endoint: string,
    body: any,
    callBack: (res: T) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {
    this.#http.post<Result<T>>(endoint, body).subscribe({
      next: (res) => {
        callBack(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#err.handle(err);
        if (errorCallBack) {
          errorCallBack(err);
        }
      },
    });
  }

  put<T>(
    endoint: string,
    body: any,
    callBack: (res: T) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {
    this.#http.put<Result<T>>(endoint, body).subscribe({
      next: (res) => {
        callBack(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#err.handle(err);
        if (errorCallBack) {
          errorCallBack(err);
        }
      },
    });
  }

  delete<T>(
    endoint: string,
    callBack: (res: T) => void,
    errorCallBack?: (err: HttpErrorResponse) => void
  ) {
    this.#http.delete<Result<T>>(endoint).subscribe({
      next: (res) => {
        callBack(res.data!);
      },
      error: (err: HttpErrorResponse) => {
        this.#err.handle(err);
        if (errorCallBack) {
          errorCallBack(err);
        }
      },
    });
  }
}
