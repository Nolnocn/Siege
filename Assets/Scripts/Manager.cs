using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
	public Character characterPrefab;

	public Transform characterContainer;
	public Transform obstacleContainer;

	private Obstacle[] m_obstacles;
	private List<Transform> m_characters;

	void Start()
	{
		InitObstacles();
		InitCharacters();
	}

	private void InitObstacles()
	{
		m_obstacles = new Obstacle[ obstacleContainer.childCount ];
		for( int i = 0; i < m_obstacles.Length; i++ )
		{
			Transform obsTrans = obstacleContainer.GetChild( i );
			m_obstacles[ i ] = obsTrans.GetComponent<Obstacle>();
		}
	}

	private void InitCharacters()
	{
		m_characters = new List<Transform>();
		for( int i = 0; i < characterContainer.childCount; i++ )
		{
			Transform character = characterContainer.GetChild( i );
			Character c = character.GetComponent<Character>();
			InitCharacter( c );
			m_characters.Add( character );
		}
	}

	private void AddCharacter()
	{
		Character newCharacter = Instantiate( characterPrefab );
		InitCharacter( newCharacter );
		m_characters.Add( newCharacter.transform );
		newCharacter.transform.parent = characterContainer;
	}

	private void InitCharacter( Character c )
	{
		c.SetObstacles( m_obstacles );
		c.SetCharacters( m_characters );
	}
}
