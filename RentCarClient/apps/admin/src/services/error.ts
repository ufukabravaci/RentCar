import { HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { FlexiToastService } from 'flexi-toast';

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  readonly #toast = inject(FlexiToastService);

  handle(err: HttpErrorResponse) {
    const status = err.status;

    if (status === 403 || status === 422 || status === 500) {
      const messages: string[] = err.error.errorMessages;
      if (Array.isArray(messages)) {
        messages.forEach(msg =>
          this.#toast.showToast('Hata!', msg, 'error')
        );
      } else if (typeof messages === 'string') {
        this.#toast.showToast('Hata!', messages, 'error');
      }
    }
  }
}