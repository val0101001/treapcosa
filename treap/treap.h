#include <iostream>
#include <limits>
#include <climits>
#include <random>
#include <cstdlib>

using namespace std;

template <typename Comparable>
class UniformRandom {
public:
    UniformRandom() : generator(random_device()()), distribution(0, numeric_limits<int>::max()) {}

    int nextInt(int min = 0, int max = numeric_limits<int>::max()) {
        if (min > max)
            swap(min, max);
        uniform_int_distribution<int> intDistribution(min, max);
        return intDistribution(generator);
    }

private:
    default_random_engine generator;
    uniform_int_distribution<int> distribution;
};

template <typename Comparable>
class Treap {
private:
    struct TreapNode {
        Comparable element;
        TreapNode *left;
        TreapNode *right;
        int priority;

        TreapNode() : left{nullptr}, right{nullptr}, priority{INT_MAX} {}

        TreapNode(const Comparable &e, TreapNode *lt, TreapNode *rt, int pr)
            : element{e}, left{lt}, right{rt}, priority{pr} {}

        TreapNode(Comparable &&e, TreapNode *lt, TreapNode *rt, int pr)
            : element{move(e)}, left{lt}, right{rt}, priority{pr} {}
    };

    TreapNode *root;
    TreapNode *nullNode;
    UniformRandom<Comparable> randomNums;

    void rotateWithLeftChild(TreapNode *&k2) {
        TreapNode *k1 = k2->left;
        k2->left = k1->right;
        k1->right = k2;
        k2 = k1;

        k1->right->priority = k2->priority;
        k2->priority = max(k1->left->priority, k1->right->priority) + 1;
    }

    void rotateWithRightChild(TreapNode *&k1) {
        TreapNode *k2 = k1->right;
        k1->right = k2->left;
        k2->left = k1;
        k1 = k2;

        k2->left->priority = k1->priority;
        k1->priority = max(k2->right->priority, k2->left->priority) + 1;
    }

    TreapNode *clone(const TreapNode *t, TreapNode *parent) const {
        if (t == nullNode) {
            return nullNode;
        } else {
            TreapNode *newNode = new TreapNode(*t);  // Use copy constructor
            newNode->left = clone(t->left, newNode);
            newNode->right = clone(t->right, newNode);
            return newNode;
        }
    }

    void makeEmpty(TreapNode *&t) {
        if (t != nullNode) {
            makeEmpty(t->left);
            makeEmpty(t->right);
            delete t;
        }
        t = nullNode;
    }

    void insert(const Comparable &x, TreapNode *&t) {
        if (t == nullNode)
            t = new TreapNode{x, nullNode, nullNode, randomNums.nextInt()};
        else if (x < t->element) {
            insert(x, t->left);
            if (t->left->priority < t->priority)
                rotateWithLeftChild(t);
        } else if (t->element < x) {
            insert(x, t->right);
            if (t->right->priority < t->priority)
                rotateWithRightChild(t);
        }
    }

    void remove(const Comparable &x, TreapNode *&t) {
        if (t != nullNode) {
            if (x < t->element)
                remove(x, t->left);
            else if (t->element < x)
                remove(x, t->right);
            else {
                if (t->left == nullNode) {
                    TreapNode *oldNode = t;
                    t = t->right;
                    delete oldNode;
                } else if (t->right == nullNode) {
                    TreapNode *oldNode = t;
                    t = t->left;
                    delete oldNode;
                } else {
                    if (t->left->priority < t->right->priority) {
                        rotateWithLeftChild(t);
                        remove(x, t->right);
                    } else {
                        rotateWithRightChild(t);
                        remove(x, t->left);
                    }
                }
            }
        }
    }

    bool contains(const Comparable &x, TreapNode *t) const {
        if (t == nullNode) {
            return false;
        } else if (x < t->element) {
            return contains(x, t->left);
        } else if (t->element < x) {
            return contains(x, t->right);
        } else {
            return true;
        }
    }

    void displayTreeStructure(TreapNode *t, int depth = 0) const {
        if (t != nullNode) {
            displayTreeStructure(t->right, depth + 1);
            for (int i = 0; i < depth; ++i) {
                cout << "\t";
            }
            cout << t->element << " (Priority: " << t->priority << ")" << endl;
            displayTreeStructure(t->left, depth + 1);
        }
    }

public:
    Treap() {
        nullNode = new TreapNode;
        nullNode->left = nullNode->right = nullNode;
        nullNode->priority = INT_MAX;
        root = nullNode;
    }

    Treap(const Treap &rhs) {
        nullNode = new TreapNode;
        nullNode->left = nullNode->right = nullNode;
        nullNode->priority = INT_MAX;
        root = clone(rhs.root, nullNode);
    }

    Treap(Treap &&rhs) : root{rhs.root}, nullNode{rhs.nullNode}, randomNums{rhs.randomNums} {
        rhs.root = rhs.nullNode;
        rhs.nullNode = nullptr;
    }

    ~Treap() {
        makeEmpty(root);
        delete nullNode;
    }

    Treap &operator=(const Treap &rhs) {
        if (this != &rhs) {
            makeEmpty(root);
            root = clone(rhs.root, nullNode);
        }
        return *this;
    }

    Treap &operator=(Treap &&rhs) {
        if (this != &rhs) {
            Treap tmp(move(rhs));  
            swap(root, tmp.root);
            swap(nullNode, tmp.nullNode);
            swap(randomNums, tmp.randomNums);
        }
        return *this;
    }

    void insert(const Comparable &x) {
        insert(x, root);
    }

    void remove(const Comparable &x) {
        remove(x, root);
    }

    bool contains(const Comparable &x) const {
        return contains(x, root);
    }

    void displayTreeStructure() const {
        displayTreeStructure(root);
    }
};
