import { Injectable } from '@angular/core';
import { FriendEditComponent } from '../friends/friend-edit/friend-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<FriendEditComponent> {
    canDeactivate(component: FriendEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
        }
        return true;
    }
}