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

  ngOnInit(): void {
    this.loadEventPlacements();
  }

  loadEventPlacements(): void {
    this.httpClient.get<EventPlacement[]>('/api/eventplacement/available').subscribe((data: any) => {
      this.eventplacements = Array.isArray(data) ? data : [data];
    });
  }

  reservEeventplacement(id: string): void {
    this.httpClient.post(`/api/cart/add/${id}`, {}).subscribe({
      next: (data: any) => {
        let index = this.eventplacements.findIndex(eventplacement => eventplacement.id === id);
        if (index !== -1) {
          this.eventplacements[index].available = false;
        }
        alert('Seat reserved!');
      },
      error: (err) => {
        if (err.status === 409) { // Conflict
          alert('This seat is already reserved!');
          this.loadEventPlacements(); // Reload the event placements to refresh the page
        } else {
          alert('An error occurred while reserving the seat.');
        }
      }
    });
  }
}
