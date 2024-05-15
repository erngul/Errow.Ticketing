import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-countdown-timer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './countdown-timer.component.html',
  styleUrls: ['./countdown-timer.component.css']
})
export class CountdownTimerComponent implements OnInit, OnDestroy {
  @Input() dDay!: Date;
  @Output() countdownFinished = new EventEmitter<void>();
  private subscription: Subscription = new Subscription();

  public timeDifference: number = 0;
  public seconds: number = 0;
  public minutes: number = 0;
  public hours: number = 0;
  public days: number = 0;

  private getTimeDifference() {
    const now = new Date().getTime();
    const due = new Date(this.dDay).getTime();
    this.timeDifference = due - now;

    if (this.timeDifference <= 0) {
      this.timeDifference = 0;
      this.seconds = 0;
      this.minutes = 0;
      this.hours = 0;
      this.days = 0;
      this.countdownFinished.emit(); // Emit the event when the countdown reaches zero
    } else {
      this.allocateTimeUnits(this.timeDifference);
    }
  }

  private allocateTimeUnits(timeDifference: number) {
    this.seconds = Math.floor((timeDifference / 1000) % 60);
    this.minutes = Math.floor((timeDifference / (1000 * 60)) % 60);
    this.hours = Math.floor((timeDifference / (1000 * 60 * 60)) % 24);
    this.days = Math.floor(timeDifference / (1000 * 60 * 60 * 24));
  }

  ngOnInit(): void {
    this.subscription = interval(1000).subscribe(() => {
      this.getTimeDifference();
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
