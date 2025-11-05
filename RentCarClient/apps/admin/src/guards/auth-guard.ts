import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  // Standart claims
  iss?: string; // (Issuer) – Token’ı kim oluşturdu
  sub?: string; // (Subject) – Token kime ait
  aud?: string | string[]; // (Audience) – Token’ın geçerli olduğu hedef(ler)
  exp?: number; // (Expiration time) – Token geçerlilik süresi (epoch time)
  nbf?: number; // (Not before) – Bu zamandan önce token geçerli değildir
  iat?: number; // (Issued at) – Token’ın oluşturulma zamanı
  jti?: string; // (JWT ID) – Token’a özgü benzersiz ID

  // Uygulamaya özgü claims
  userId?: string | number;
  email?: string;
  name?: string;
  role?: string;
  permissions?: string[];
  [key: string]: unknown; // yeni claim geldiğinde kod kırılmaz
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
