/************************
 *    CarControl PIC Software   *
 *     for PM2 School Project    *
 *                                       *
 ************************/

#include <pic.h>

/*
    Input ports that will receive 2 band radio signal 
 
    If Data0 == 1, we have received a ZERO
    If Data1 == 1, we have received a ONE
*/

#define Data0 RB0
#define Data1 RB1


/* Main */
void main()
{
    
             /* Our data packet received will be stored in this variable */
	char data;

             /* 
                 Initialize port B pin directions (00000011)
                 RB0 and RB1 are kept for input of the radio signals, and
                 RB6, RB7 are kept for directional engine control,
                 RB4, RB5 are kept for main engine control,
                 RB3 is used to indicate a new packet received status. (LED) 
             */
	TRISB=0x03;

             /* Initialize Timer for high limit 100ms. See datasheet for reference. */
	T1CON=0B00111100;

             /* Clear out RB2-RB7 output pins. Stop engines and LED's, etc. */
	PORTB=0;
	
             /* Main Program Loop. Accepts packets and controls the engines. */
	for(;;)
	{
                          /* If there is a 1 on Data0, we have a ZERO received from the transmitter. */
		if(Data0)
		{
                                      /* Shift packet byte (adding a zero at the end) */
			data=data<<1;

                                      /* Disable/Reset the Timer */
			TMR1ON=0;
			TMR1H=0;
			TMR1L=0;

                                      /* Wait until Data0 clears out to indicate bit transmission end. */
			while(Data0==1);

                                      /* Enable timer (waits for packet end). */
			TMR1ON=1;
		}

                          /* If there is a 1 on Data1, we have a ONE received from the transmitter. */
		if(Data1)
		{
                                      /* Shift packet byte (adding a one at the end) */
			data=(data<<1)+1;

                                      /* Disable/Reset the Timer */
			TMR1ON=0;
			TMR1H=0;
			TMR1L=0;

                                      /* Wait until Data1 clears out to indicate bit transmission end. */
			while(Data1==1);

                                      /* Enable timer (waits for packet end). */
			TMR1ON=1;
		}
                         
                          /* Timer has signaled it reached 100ms! That means we have reached the end of a packet. (No more data came after 100 ms). */
		if(TMR1IF)
		{
			/* Invert LED's status to mark the arrival of a new packet. */
			RB3=!RB3;

			/* Reset the timer flagged state and switch it off. */
			TMR1IF=0;
			TMR1ON=0;

			/* Keep only the first 3 bits (just in case we have a background noise). */
			data=data&7;
			
			/* Interpreting the results. We have a 3-bit packet that defines the action we want to perform. */
			switch(data)
			{
                                                   /* Direction Engine: Stop */
				case 1: 
				{
                                                                /* Give 0 on both Engine gates (Stop) */
					RB7=0;
					RB6=0;

					break;
				}

                                                   /* Direction Engine: Left */
				case 2:
				{
                                                                /* Give 1/0 on Engine gates (Rotates engine to the Left) */
					RB7=1;
					RB6=0;

					break;
				}

                                                   /* Direction Engine: Right */
				case 3:
				{
                                                                /* Give 0/1 on Engine gates (Rotates engine to the Right) */
					RB7=0;
					RB6=1;

					break;
				}

                                                   /* Main Engine: Stop */
				case 4:
				{
                                                                /* Give 0 on both Engine gates (Stop) */
					RB4=0;
					RB5=0;

					break;
				}

                                                   /* Main Engine: Forward */
				case 5:
				{
                                                                /* Give 1/0 on Engine gates (Rotates engine Forward) */
					RB4=1;
					RB5=0;

					break;
				}

                                                   /* Main Engine: Backward */
				case 6:
				{
                                                                /* Give 0/1 on Engine gates (Rotates engine Backward) */
					RB4=0;
					RB5=1;

					break;
				}
				
			}
		}

	}
}
