import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-friend-card',
  templateUrl: './friend-card.component.html',
  styleUrls: ['./friend-card.component.css']
})
export class FriendCardComponent implements OnInit {

  @Input() user: User;

  constructor() { }

  ngOnInit() {
  }

}
