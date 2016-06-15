#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <omp.h>
#include <iostream>

using namespace std;

unsigned long long int N = 100000000;

long double CountPi()
{
	long double a_n, temp = 0;
	long long int i;

	for (i = 0; i < N; i++)
	{
		if (i % 2 == 0)
		{
			a_n = (1.0 / (2 * i + 1));
		}
		else
		{
			a_n = (-1.0 / (2 * i + 1));
		}
		temp = temp + a_n;
	}
	return (long double)(4 * temp);
}

long double licz_pi_omp()
{
	long double a_n, temp = 0;
	long long int i;

#pragma omp parallel for default(none) private(a_n, i) reduction(+:temp) num_threads(8)
	for (i = 0; i < N; i++)
	{
		if (i % 2 == 0)
		{
			a_n = (1.0 / (2 * i + 1));
		}
		else
		{
			a_n = (-1.0 / (2 * i + 1));
		}
		temp = temp + a_n;
	}
	return (long double)(4 * temp);
}

int main()
{
	clock_t start, stop;
	long double wynik;

	start = clock();
	wynik = CountPi();
	stop = clock();

	printf("Pi=%.30lf\t", wynik);
	printf("Czas bez OpenMP:\t%dms\n", stop - start);

	start = clock();
	wynik = licz_pi_omp();
	stop = clock();
	printf("Pi=%.30lf\t", wynik);
	printf("Czas z OpenMP:\t\t%dms\n", stop - start);

	system("pause");
	return 0;
}