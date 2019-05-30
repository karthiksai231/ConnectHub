import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { FriendListComponent } from './friends/friend-list/friend-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { FriendDetailComponent } from './friends/friend-detail/friend-detail.component';
import { FriendDetailResolver } from './_resolvers/friend-detail.resolver';
import { FriendListResolver } from './_resolvers/friend-list.resolver';
import { FriendEditComponent } from './friends/friend-edit/friend-edit.component';
import { FriendEditResolver } from './_resolvers/friend-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes,guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    { 
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'friends', component: FriendListComponent, resolve: {users: FriendListResolver}},
            { path: 'friends/:id', component: FriendDetailComponent, resolve: {user: FriendDetailResolver}},
            {path: 'friend/edit', component: FriendEditComponent,
                resolve: {user: FriendEditResolver}, canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
            { path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}}
        ]
    },
    { path: '**', redirectTo: 'home', pathMatch: 'full'}
];
