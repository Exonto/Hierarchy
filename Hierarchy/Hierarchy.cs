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
		private Node<E> _rootNode;

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

		/// <summary>
		/// Returns a mutable list of the child <b>elements</b> of the given
		/// parent.
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
		/// Returns the child nodes of the given element.
		/// </summary>
		/// <returns>The child nodes.</returns>
		/// <param name="parent">Parent.</param>
		public List<Node<E>> GetChildNodes(E parent)
		{
			Node<E> parentNode = this.GetNode(parent, this._rootNode);

			if (parentNode != null)
			{
				return parentNode.ChildNodes;
			}
			else {
				return null;
			}
		}

		/// <summary>
		/// Returns a node which is located based upon matching elements.
		/// This will start at the root node and search from that point.
		/// </summary>
		/// <returns>The child node.</returns>
		/// <param name="element">Element.</param>
		public Node<E> GetNode(E element)
		{
			// Begins searching at the root node
			return this.GetNode(element, this._rootNode);
		}

		/// <summary>
		/// Returns a node which is located based upon matching elements.
		/// 
		/// The start node indicates the node whose children will be
		/// searched through for a matching pair. To check the entire
		/// <c>Hierarchy</c>, use the root node.
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

				// If this node does not itself contain the element 
				// being searched for, then it will instead recursively
				// search through its own children.
				foreach (Node<E> child in startNode.ChildNodes)
				{
					Node<E> foundNode = this.GetNode(element, child);
					if (foundNode != null) return foundNode;
				}
			}

			// No node was found
			return null;
		}

		/// <summary>
		/// This allows the Hierarchy to have a "nested" or "sub" Hierarchy
		/// extracted from it.
		/// This is allowed because all Hierarchies are composed of nothing
		/// more than other smaller Hierarchies.
		/// 
		/// The sub Hierarchy that is returned is a copy and changes made to 
		/// this copy will not affect the original.
		/// </summary>
		/// <returns>The nested.</returns>
		/// <param name="element">Element.</param>
		public Hierarchy<E> GetNested(E element)
		{
			Node<E> startNode = this.GetNode(element);
			Hierarchy<E> nested = new Hierarchy<E>(element);
			return this.GetNested(nested, nested._rootNode,  startNode);
		}

		/// <summary>
		/// This is an internal helper method which handles most of the actual
		/// logic for Hierarchy.GetNested(E element).
		/// </summary>
		/// <returns>The nested.</returns>
		/// <param name="nested">Nested.</param>
		/// <param name="nestedSentry">Nested sentry.</param>
		/// <param name="originalSentry">Original sentry.</param>
		private Hierarchy<E> GetNested(Hierarchy<E> nested, 
		                               Node<E> nestedSentry, 
		                               Node<E> originalSentry)
		{
			// This method takes two nodes: Nested Sentry and Original Sentry.
			// The Nested Sentry represents the node currently being observed in
			// the Nested Hierarchy which is being built.
			// The Original Sentry represents the node currently being observed in
			// the Original Hierachy from which the Nested Hierarchy is being built
			// from.
			//
			// These two nodes allow for quick copying and, more importantly, traversal
			// of both Hierarchies. The Nested Sentry node's job is to "follow" the
			// Original Sentry around and copy over that sentry's element.

			// Confirms that the original sentry it not null
			if (originalSentry == null) { return null; }

			// Goes through each of the original sentry's child nodes
			foreach (Node<E> child in originalSentry.ChildNodes)
			{
				// Adds the current child's element to the nested hierarchy
				// via the nested sentry
				Node<E> newNestedSentry = nestedSentry.AddChild(child.Element);

				this.GetNested(nested, newNestedSentry, child);
			}

			return nested;
		}

// -----------------------------------------------------------------------------
// Hierarchy Properties
// -----------------------------------------------------------------------------

		/// <summary>
		/// Returns the root element of this <c>Hierarchy</c>.
		/// </summary>
		/// <value>The root.</value>
		public E Root
		{
			get { return this._rootNode.Element; }
			set { this._rootNode.Element = value; }
		}

		/// <summary>
		/// Gets the root node of this <c>Hierarchy</c>.
		/// 
		/// </summary>
		/// <value>The root node.</value>
		public Node<E> RootNode
		{
			get { return this._rootNode; }
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
			/// This field should not be accessible from outside the containing 
			/// class.
			/// </summary>
			private Node<E> _parent;

			private E _element;

			private List<Node<E>> _children = new List<Node<E>>();

			// Keeps a cached record of the elements contained within this node's
			// child nodes. This is done so that acquiring these elements from
			// their respective nodes is a quick process
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
			/// Can add a variable number of children to this node.
			/// </summary>
			/// <param name="elements">Elements.</param>
			public void AddChildren(params E[] elements)
			{
				foreach (E element in elements)
				{
					this.AddChild(element);
				}
			}

			/// <summary>
			/// Will add all of the given elements to this node as children.
			/// </summary>
			/// <param name="elements">Elements.</param>
			public void AddChildren(List<E> elements)
			{
				foreach (E element in elements)
				{
					this.AddChild(element);
				}
			}

			/// <summary>
			/// Returns whether this node has no children.
			/// </summary>
			/// <returns><c>True</c>, if node has no children, <c>false</c> otherwise.</returns>
			public bool IsLeaf()
			{
				return this._children.Count == 0;
			}

			/// <summary>
			/// Returns whether or not this node contains a child node which 
			/// contains the given element.
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

// -----------------------------------------------------------------------------
// Node Properties
// -----------------------------------------------------------------------------

			/// <summary>
			/// Gets or sets the element.
			/// </summary>
			/// <value>The element.</value>
			public E Element
			{
				get { return this._element; }
				set { this._element = value; }
			}

			/// <summary>
			/// Returns a list of this node's children's elements.
			/// </summary>
			/// <value>The children.</value>
			public List<E> Children
			{
				// Returns the elements contained within each child node by
				// accessing the cache of these elements rather than looping
				// through each child node
				get { return cachedChildElements; }
			}

			/// <summary>
			/// Returns a list of this node's child nodes.
			/// </summary>
			/// <value>The child nodes.</value>
			public List<Node<E>> ChildNodes
			{
				get { return this._children; }
			}

			/// <summary>
			/// Returns this node's parent node.
			/// </summary>
			/// <value>The parent.</value>
			public Node<E> Parent
			{
				get { return this._parent; }
			}
		}
	}
}
