#include <iostream>
#include <thread>
#include <omp.h>
#include <ctime>
#include <cstdlib>

using namespace std;

const int N = 500;
int P = 50;//podaæ w procentach tj. np. 95 zamiast 0.95

class gnp
{
public:
	int EdgesCount = 0;
	int *VertexDegree;
	int temp = 0;
	int ** Graph;
	gnp()
	{
		VertexDegree = new int[N];
		Graph = new int *[N];
		for (int i = 0; i < N; i++)
		{
			Graph[i] = new int[N];
		}

		int i, j;
#pragma omp parallel for private (i,j)
		for (i = 0; i < N; i++)
		{
			for (j = i + 1; j < N; j++)
			{
				int losowa = (rand() % 100);
				if (P >= losowa)
				{
					Graph[i][j] = 1;
					Graph[j][i] = 1;
				}
				else
				{
					Graph[i][j] = 0;
					Graph[j][i] = 0;
				}
			}
			Graph[i][i] = 0;
		}
	}
	~gnp() {}
	void ShowGraph()
	{
		for (int i = 0; i < N; i++)
		{
			for (int j = 0; j < N; j++)
			{
				cout << Graph[i][j] << "";
			}
			cout << "\t" << VertexDegree[i] << endl;
		}
	}
	int CountEdges()
	{
		EdgesCount = 0;
		int i, j;
#pragma	omp parallel for private(i,j)
		for (i = 0; i<N; i++)
			for (j = i + 1; j<N; j++)
				if (Graph[i][j])
					EdgesCount++;
		return EdgesCount;
	}
	void CountVertexDegree()
	{
		int i, j;
#pragma	omp parallel for private(i,j)
		for (i = 0; i < N; i++)
		{
			VertexDegree[i] = 0;
			for (j = 0; j < N; j++)
			{
				if (Graph[i][j] == 1)
				{
					VertexDegree[i]++;
				}
			}
		}
	}
};

bool CompareGraphs(gnp graf1, gnp graf2)
{
	if (graf1.CountEdges() == graf2.CountEdges())//sprawdzenie ilosci krawedzi, rozna ilosc krawedzi wskazuje na nie izomorficznosc grafow
	{
		bool wynik = true;
#pragma omp parallel for
		for (int i = 0; i < N; i++)//sprawdzenie czy grafy sa identyczne
		{
			for (int j = i + 1; j < N; j++)
			{
				if (graf1.Graph[i][j] != graf2.Graph[i][j])
				{
					wynik = false;
					break;
				}
			}
		}
		if (wynik)
		{
			cout << "Grafy sa identyczne, wiec ";
			return wynik;
		}
		graf1.CountVertexDegree();
		graf2.CountVertexDegree();
		for (int i = 0; i < N; i++)//sprawdzenie czy grafy maja te same stopnie wierzcholkow, jesli tak to po permutacji grafy beda izomorficzne
		{
			bool temp = false;
			for (int j = 0; j < N; j++)
			{
				if (graf1.VertexDegree[i] == graf2.VertexDegree[j])
				{
					temp = true;
				}
			}
			if (!temp)
			{
				wynik = false;
				break;
			}
			else
			{
				wynik = true;
			}
		}
		if (wynik)
		{
			cout << "Macierze sasiedztwa sie nie zgadzaja, ale po permutacjach ";
		}
		return wynik;
	}
	else
	{
		cout << "Ilosc krawedzi grafow sie nie zgadza (" << graf1.EdgesCount << " " << graf2.EdgesCount << "), wiec ";
		return false;
	}
}

int main()
{
	srand(time(NULL));

	gnp moj_graf1 = gnp();
	gnp moj_graf2 = gnp();
	moj_graf1.CountVertexDegree();
	moj_graf2.CountVertexDegree();
	
	double start, stop;

	if (CompareGraphs(moj_graf1, moj_graf2))
	{
		cout << "grafy sa izomorficzne" << endl;
	}
	else
	{
		cout << "grafy nie sa izomorficzne" << endl;
	}

	system("pause");
	return 0;
}