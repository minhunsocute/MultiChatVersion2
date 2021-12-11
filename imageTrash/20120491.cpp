#include<iostream>
#include<cmath>
using namespace std;

float **arr2D_Alloc(int m)
{
    float **arr = nullptr;
    arr = new (std::nothrow) float *[m];
    for (int i = 0; i < m; i++)
    {
        arr[i] = new (std::nothrow) float[m];
    }
    return arr;
}
void input_RowCOlums(int&m)
{
    cout<<"Input rows and colums: ";
    while(m<=0)
    {
        cin>>m;
        if(m<=0)
        {
            cout<<"Row must > 0 input again:";
        }
        cout<<"\n";
    }   
    
}
void delete_Array(float**&arr,int m)
{
    for(int i=0;i<m;i++)
    {
        delete[] arr[i];
    }
    delete[] arr;
}
void input_matrix(float**&arr,int m)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            cout<<"Arr["<<i<<"]"<<"["<<j<<"]:";
            cin>>arr[i][j];
        }
    }
}
void print_matrix(float**arr,int m)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            cout<<arr[i][j]<<"  ";
        }
        cout<<"\n";
    }
}
float medium(float**arr,int m)
{   
    float medium=0;
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            medium+=arr[i][j];
        }
    }
    return medium/(int(m*m));
}
float return_nearest(float **arr, int m)
{
    float min=abs(medium(arr,m)-arr[0][0]);
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            if(min>abs(medium(arr,m)-arr[i][j]))
            {
                min=abs(medium(arr,m)-arr[i][j]);
            }
        }
    }
    return min;
}
void find_coordinatesNearsestMedium(float**arr,int m,int&x,int&y)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            if(abs(medium(arr,m)-arr[i][j])==return_nearest(arr,m))
            {
                x=i;y=j;
                return;
            }
        }
    }
}
void Sort_up_ascending(float**&arr,int m)
{
    for(int i=0;i<m-1;i++)
    {
        for(int j=i+1;j<m;j++)
        {
            if(arr[i][i]>arr[j][j])
            {
                swap(arr[i][i],arr[j][j]);
            }
        }
    }
}

int main()
{
    int m=0;
    input_RowCOlums(m);
    float** arr= nullptr;
    arr=arr2D_Alloc(m);
    input_matrix(arr,m);
    cout<<"Matrix: "<<"\n";
    print_matrix(arr,m);
    int coordinatesx=0;
    int coordinatesy=0;
    find_coordinatesNearsestMedium(arr,m,coordinatesx,coordinatesy);
    cout<<"nearest coordinates are: ("<<coordinatesx<<","<<coordinatesy<<")"<<":"<<arr[coordinatesx][coordinatesy]<<"\n";
    Sort_up_ascending(arr,m);
    cout<<"Matrix After sort: "<<"\n";
    print_matrix(arr,m);
    delete_Array(arr,m);
}#include<iostream>
#include<cmath>
using namespace std;

float **arr2D_Alloc(int m)
{
    float **arr = nullptr;
    arr = new (std::nothrow) float *[m];
    for (int i = 0; i < m; i++)
    {
        arr[i] = new (std::nothrow) float[m];
    }
    return arr;
}
void input_RowCOlums(int&m)
{
    cout<<"Input rows and colums: ";
    while(m<=0)
    {
        cin>>m;
        if(m<=0)
        {
            cout<<"Row must > 0 input again:";
        }
        cout<<"\n";
    }   
    
}
void delete_Array(float**&arr,int m)
{
    for(int i=0;i<m;i++)
    {
        delete[] arr[i];
    }
    delete[] arr;
}
void input_matrix(float**&arr,int m)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            cout<<"Arr["<<i<<"]"<<"["<<j<<"]:";
            cin>>arr[i][j];
        }
    }
}
void print_matrix(float**arr,int m)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            cout<<arr[i][j]<<"  ";
        }
        cout<<"\n";
    }
}
float medium(float**arr,int m)
{   
    float medium=0;
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            medium+=arr[i][j];
        }
    }
    return medium/(int(m*m));
}
float return_nearest(float **arr, int m)
{
    float min=abs(medium(arr,m)-arr[0][0]);
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            if(min>abs(medium(arr,m)-arr[i][j]))
            {
                min=abs(medium(arr,m)-arr[i][j]);
            }
        }
    }
    return min;
}
void find_coordinatesNearsestMedium(float**arr,int m,int&x,int&y)
{
    for(int i=0;i<m;i++)
    {
        for(int j=0;j<m;j++)
        {
            if(abs(medium(arr,m)-arr[i][j])==return_nearest(arr,m))
            {
                x=i;y=j;
                return;
            }
        }
    }
}
void Sort_up_ascending(float**&arr,int m)
{
    for(int i=0;i<m-1;i++)
    {
        for(int j=i+1;j<m;j++)
        {
            if(arr[i][i]>arr[j][j])
            {
                swap(arr[i][i],arr[j][j]);
            }
        }
    }
}

int main()
{
    int m=0;
    input_RowCOlums(m);
    float** arr= nullptr;
    arr=arr2D_Alloc(m);
    input_matrix(arr,m);
    cout<<"Matrix: "<<"\n";
    print_matrix(arr,m);
    int coordinatesx=0;
    int coordinatesy=0;
    find_coordinatesNearsestMedium(arr,m,coordinatesx,coordinatesy);
    cout<<"nearest coordinates are: ("<<coordinatesx<<","<<coordinatesy<<")"<<":"<<arr[coordinatesx][coordinatesy]<<"\n";
    Sort_up_ascending(arr,m);
    cout<<"Matrix After sort: "<<"\n";
    print_matrix(arr,m);
    delete_Array(arr,m);
}