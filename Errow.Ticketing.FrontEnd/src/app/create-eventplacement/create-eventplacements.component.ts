import { HttpClient } from '@angular/common/http';
import {Component, inject, Injectable} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-eventplacements',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './create-eventplacements.component.html',
  styleUrls: ['./create-eventplacements.component.css']
})

export class CreateeventplacementsComponent {
  httpClient: HttpClient = inject(HttpClient);

  initializeeventplacements() {
    console.log('test');
    this.httpClient.post<any>('/api/eventplacement/initialize', null).subscribe({complete: console.info});
    console.log('eventplacements initialized')

  }
}
