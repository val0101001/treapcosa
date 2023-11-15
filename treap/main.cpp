#include "treap.h"  
using namespace std;

//-------------------TEST-------------------//
int main() {
    Treap<int> treap;

    // insert
    treap.insert(5);
    treap.insert(3);
    treap.insert(8);
    treap.insert(2);
    treap.insert(4);
    treap.insert(7);
    treap.insert(9);

    // Find
    cout << "Contains 4: " << (treap.contains(4) ? "Yes" : "No") << endl;
    cout << "Contains 6: " << (treap.contains(6) ? "Yes" : "No") << endl;

    // Remove
    treap.remove(3);

    // Find removed
    cout << "Contains 3: " << (treap.contains(3) ? "Yes" : "No") << endl;

    // Display tree structure
    cout << endl << "Tree structure:" << endl;
    treap.displayTreeStructure();
    return 0;
}

//------------------------------------------//
