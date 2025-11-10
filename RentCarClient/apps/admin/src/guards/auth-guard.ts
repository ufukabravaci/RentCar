import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  exp?: number;
  userId?: string;
  fullName?: string;
  email?: string;
}
export const authGuard: CanActivateChildFn = () => {
  const token = localStorage.getItem('response');
  const router = inject(Router);
  if (!token) {
    router.navigateByUrl('/login');
    return false;
  }
  try {
    const decode = jwtDecode<JwtPayload>(token);
    const now = new Date().getTime() / 1000;
    const expire = decode.exp ?? 0;
    if (expire <= now) {
      router.navigateByUrl('/login');
      return false;
    }
    return true;
  } catch (error) {
    router.navigateByUrl('/login', { info: error });
    return false;
  }
};
