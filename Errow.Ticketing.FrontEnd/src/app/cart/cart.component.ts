import {Component, inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  httpClient: HttpClient = inject(HttpClient);
  data: string = '';

  ngOnInit(): void {
    this.httpClient.get<any>('/api/cart').subscribe((data: any) => {
      console.log(data)
      data = data.result
    });
  }

}
