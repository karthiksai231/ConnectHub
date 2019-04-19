import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class FriendDetailResolver implements Resolve<User> {

    constructor(private userService: UserService, private router: Router,
        private alert: AlertifyService) {}

        resolve(route: ActivatedRouteSnapshot): Observable<User> {
            return this.userService.getUser(route.params['id']).pipe(
                catchError(error => {
                    this.alert.error('Unable to retrieve data');
                    this.router.navigate(['/friends']);
                    return of(null);
                })
            )
        }
}