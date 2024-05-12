import {Component, inject, Injectable, OnInit} from '@angular/core';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {CommonModule} from '@angular/common';
import {HttpClient} from "@angular/common/http";
import {MatCard, MatCardContent} from "@angular/material/card";

interface EventPlacement {
  id: string;
  row: number;
  column: number;
  available: boolean;
}

@Component({
  selector: 'app-eventplacement-component',
  standalone: true,
  imports: [CommonModule, MatProgressBarModule, MatCardContent, MatCard],
  templateUrl: './eventplacement.component.html',
  styleUrl: './eventplacement.component.css'
})
export class eventplacementComponent implements OnInit {
  eventplacements: EventPlacement[] = [];
  httpClient: HttpClient = inject(HttpClient);
  color = 'primary';
  mode = 'Query';
  value = 50;
  bufferValue = 75;
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
