import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }
  registerToggle() {
    this.registerMode = true;
  }

  chatToggle() {
    const botDiv = document.getElementById('botDiv');
    botDiv.style.height = botDiv.style.height === '600px' ? '38px' : '600px';
  }

  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }

}
