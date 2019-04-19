import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { FriendListComponent } from './friends/friend-list/friend-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { FriendDetailComponent } from './friends/friend-detail/friend-detail.component';
import { FriendDetailResolver } from './_resolvers/friend-detail.resolver';
import { FriendListResolver } from './_resolvers/friend-list.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    { 
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'friends', component: FriendListComponent, resolve: {users: FriendListResolver}},
            { path: 'friends/:id', component: FriendDetailComponent, resolve: {user: FriendDetailResolver}},
            { path: 'messages', component: MessagesComponent},
            { path: 'lists', component: ListsComponent}
        ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
