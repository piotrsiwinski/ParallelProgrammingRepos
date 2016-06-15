#include <cstdio>
#include <atomic>
#include <cmath>

using namespace std;

const int numberOfNumbers = 65;

int main()
{
	int Difference[numberOfNumbers], c, Result[numberOfNumbers] = {};
	int  i, j, ToSum[numberOfNumbers][numberOfNumbers];

	ToSum[0][0] = Difference[0] = 1;

#pragma omp parallel for
	for (i = 1; i < numberOfNumbers; i++)
	{
		ToSum[0][i] = Difference[i] = 0;
	}

	for (j = 1; j<numberOfNumbers; j++)
	{
		c = 0;
#pragma omp for
		for (i = 0; i<numberOfNumbers; i++)
		{
			c = Difference[i] + c * 10;
			Difference[i] = c / j;
			c = c - Difference[i] * j;
			ToSum[j][i] = Difference[i];
		}
	}

	for (j = 0; j < numberOfNumbers; j++)
	{
#pragma omp parallel for
		for (i = 0; i < numberOfNumbers; i++)
		{
			Result[i] += ToSum[j][i];
		}
	}

	for (i = numberOfNumbers - 1; i >= 1; i--)
	{
		int temp = Result[i];
		Result[i] = Result[i] % 10;
		Result[i - 1] += floor(temp / 10);
	}

	printf("wynik to %d.", Result[0]);
#pragma omp parallel for schedule(static) ordered
	for (i = 1; i < numberOfNumbers; i++)
	{
#pragma omp ordered 
		printf("%d", Result[i]);
	}
	system("pause");
}