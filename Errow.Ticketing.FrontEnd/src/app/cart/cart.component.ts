import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { NgForOf, NgIf } from "@angular/common";
import { CommonModule } from '@angular/common';
import { CountdownTimerComponent } from '../countdown-timer/countdown-timer.component';

interface CartItem {
  eventPlacementId: string;
  dueDateTime: Date;
  CreatedDateTime: Date;
}

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    CommonModule,
    CountdownTimerComponent
  ],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  private httpClient: HttpClient = inject(HttpClient);

  cartItems: CartItem[] = [];
  public dueDate: Date = new Date('2024-12-31T23:59:59');

  ngOnInit(): void {
    this.getCartItems();
  }

  getCartItems(): void {
    this.httpClient.get<any>('/api/cart').subscribe((data: any) => {
      this.cartItems = data.result.items;
    });
  }

  removeFromCart(seatId: string): void {
    this.httpClient.post(`/api/cart/remove/${seatId}`, {}).subscribe(() => {
      this.getCartItems();
    });
  }

  onCountdownFinished(): void {
    location.reload(); // Refresh the page
  }
}
