using System;
using System.Collections.Generic;

namespace HierarchyCollection
{
	/// This represents a <c>Hierarchy</c> data structure.
	/// 
	/// This stores information with no restrictions and 
	public class Hierarchy<E>
	{	
		/// <summary>
		/// This represents the first node in the <c>Hierarchy</c>
		/// </summary>
		public Node<E> _rootNode;

		public Hierarchy() { }

		/// <summary>
		/// Creates a new <c>Hierarchy</c> whose root node has been given.
		/// </summary>
		/// <param name="rootObj">Root object from which all other elements
		/// stem.</param>
		public Hierarchy(E rootObj)
		{
			this._rootNode = new Node<E>(rootObj);
		}

		//public void setRootNode()

		private void migrateChildren(Node<E> original, Node<E> other)
		{
			foreach (Node<E> node in original._children)
			{
				
			}
		}

		/// <summary>
		/// Returns a mutable list of child elements.
		/// </summary>
		/// <returns>The children.</returns>
		/// <param name="parent">Parent.</param>
		public List<E> GetChildren(E parent)
		{
			Node<E> parentNode = this.GetNode(parent, this._rootNode);
			if (parentNode != null)
			{
				return parentNode.Children;
			} else {
				return null;
			}
		}

		/// <summary>
		/// Returns a node which is located based upon matching elements.
		/// </summary>
		/// <returns>The node.</returns>
		/// <param name="element">Element.</param>
		/// <param name="startNode">Start node.</param>
		public Node<E> GetNode(E element, Node<E> startNode)
		{
			if (startNode.Element.Equals(element))
			{
				return startNode;

              // If the start node contains the element being looked for
			} else if (startNode.IsLeaf() == false) {

				// If none of this node's children contain the element,
				// then this will recursively search through the 
				// children's children.
				foreach (Node<E> child in startNode._children)
				{
					Node<E> foundNode = this.GetNode(element, child);
					if (foundNode != null) return foundNode;
				}
			}

			// Indicates that no node was found
			return null;
		}

		/// <summary>
		/// Returns the root element of this <c>Hierarchy</c>.
		/// </summary>
		/// <value>The root.</value>
		public E Root
		{
			get { return this._rootNode.Element; }
		}
// -----------------------------------------------------------------------------

		/// <summary>
		/// Returns whether the <c>Hierarchy</c> contains is empty/conatins no
		/// information.
		/// </summary>
		/// <returns><c>true</c>, if empty, <c>false</c> otherwise.</returns>
		public bool IsEmpty()
		{
			return this._rootNode == null;
		}

// -----------------------------------------------------------------------------

		/// <summary>
		/// This private inner class assists <c>Hierarchy</c> in storing its 
		/// information.
		/// </summary>
		public class Node<E>
		{
			/// <summary>
			/// Defines the parent node which is storing this node as a "child".
			/// </summary>
			private Node<E> _parent;

			private E _element;

			// Only accessible by the outer class
			public List<Node<E>> _children = new List<Node<E>>();

			// Keeps a cached record of the elements contained within this node's
			// child nodes. This is done so that acquiring these elements is 
			// quick process
			private List<E> cachedChildElements = new List<E>();

			public Node(E element)
			{
				this.Element = element;
			}



			/// <summary>
			/// Gives this node a new child node to keep track of.
			/// This will also supply its child node a new parent.
			/// </summary>
			/// <returns>The child.</returns>
			/// <param name="element">Element.</param>
			public Node<E> AddChild(E element)
			{
				Node<E> childNode = new Node<E>(element);

				// Both _children and cachedChildElements share a reference to the
				// same element except that _children stores a node which then 
				// contains which then contains that reference
				this._children.Add(childNode);
				this.cachedChildElements.Add(element);

				childNode._parent = this;

				return childNode;
			}

			/// <summary>
			/// Returns whether this node has no children.
			/// </summary>
			/// <returns><c>false</c>, if node has children, <c>true</c> otherwise.</returns>
			public bool IsLeaf()
			{
				return this._children.Count == 0;
			}

			/// <summary>
			/// Returns whether or not this node contains a child node which contains
			/// the given element.
			/// </summary>
			/// <returns><c>true</c>, if a child has this element, <c>false</c> otherwise.</returns>
			/// <param name="element">Element.</param>
			public bool HasChild(E element)
			{
				foreach (E e in this.cachedChildElements)
				{
					if (e.Equals(element))
					{
						return true;
					}
				}

				return false;
			}

			/// <summary>
			/// Returns the child node which contains the given element.
			/// If no child exists, this will instead return <c>null</c>.
			/// </summary>
			/// <returns>The child or null if the child does not exist.</returns>
			/// <param name="element">Element.</param>
			public Node<E> GetChild(E element)
			{
				foreach (Node<E> child in this._children)
				{
					if (child.Element.Equals(element))
					{
						return child;
					}
				}

				return null;
			}


			/// <summary>
			/// Gets or sets the element.
			/// </summary>
			/// <value>The element.</value>
			public E Element
			{
				get { return this._element; }
				set { this._element = value; }
			}

			public List<E> Children
			{
				get 
				{
					// Returns the elements contained within each child node by
					// accessing the cache of these elements rather than looping
					// through each child node
					return cachedChildElements;
				}
			}
		}
	}
}
