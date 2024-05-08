import {Component, inject, Injectable, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HttpClient} from "@angular/common/http";

interface EventPlacement {
  id: string;
  row: number;
  column: number;
  available: boolean;
}

@Component({
  selector: 'app-eventplacement-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './eventplacement.component.html',
  styleUrl: './eventplacement.component.css'
})
export class eventplacementComponent implements OnInit {
  eventplacements: EventPlacement[] = [];
  httpClient: HttpClient;

  constructor(httpClient: HttpClient) {
    this.httpClient = httpClient;
  }

  ngOnInit(): void {
    this.httpClient.get<EventPlacement[]>('/api/eventplacement').subscribe((data: any) => {
      this.eventplacements = Array.isArray(data) ? data : [data];
    });
  }

  reservEeventplacement(id: string): void {
    this.httpClient.post(`/api/eventplacement/reserve/${id}`, {}).subscribe((data: any) => {
      let index = this.eventplacements.findIndex(eventplacement => eventplacement.id === id);
      if (index !== -1) {
        this.eventplacements[index].available = false;
      }
      alert('eventplacements reserved!');
    });
  }
}
