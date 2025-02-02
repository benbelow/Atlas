Feature: Four out of eight Search - single mismatch
  As a member of the search team
  I want to be able to run a 4/8 cord search
  And see results with a single mismatch

  Scenario: 4/8 Search with a single mismatch at A
    Given a patient and a donor
    And the donor has a single mismatch at locus A
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with a single mismatch at B
    Given a patient and a donor
    And the donor has a single mismatch at locus B
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with a single mismatch at DRB1
    Given a patient and a donor
    And the donor has a single mismatch at locus DRB1
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with a single mismatch at C
    Given a patient and a donor
    And the donor has a single mismatch at locus C
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with one mismatch each at locus A and DQB1
    Given a patient and a donor
    And the donor has a single mismatch at locus A
    And the donor has a single mismatch at locus DQB1
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with one mismatch each at locus B and DQB1
    Given a patient and a donor
    And the donor has a single mismatch at locus B
    And the donor has a single mismatch at locus DQB1
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with one mismatch each at locus C and DQB1
    Given a patient and a donor
    And the donor has a single mismatch at locus C
    And the donor has a single mismatch at locus DQB1
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor

  Scenario: 4/8 Search with one mismatch each at locus DRB1 and DQB1
    Given a patient and a donor
    And the donor has a single mismatch at locus DRB1
    And the donor has a single mismatch at locus DQB1
    And the donor is of type cord
    And the search type is cord
    When I run a 4/8 search
    Then the results should contain the specified donor